#include "pch.h"
#include "JScriptValueMarshaller.h"
#include "JScriptRuntime.h"

using namespace JSRT;
using namespace Platform;
using namespace Windows::UI::Xaml;

JScriptRuntime::JScriptRuntime()
{
    m_state = ref new JScriptValueMarshaller();
    IfFailError(JsCreateRuntime(JsRuntimeAttributeNone, nullptr, &m_runtime), L"failed to create runtime.");
    IfFailError(CreateHostContext(m_runtime, &m_context), L"failed to create execution context.");
error:
    ;
}

JsValueRef CALLBACK Shim(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    auto holder = (JScriptRuntimeNativeHolder*)callbackState;
    switch (holder->method)
    {
    case 0:
        return holder->ref->Echo(callee, isConstructCall, arguments, argumentCount, nullptr);
    case 1:
        return holder->ref->SetInterval(callee, isConstructCall, arguments, argumentCount, nullptr);
    case 2:
        return holder->ref->ClearInterval(callee, isConstructCall, arguments, argumentCount, nullptr);
    case 3:
        return holder->ref->SetState(callee, isConstructCall, arguments, argumentCount, nullptr);
    case 4:
        return holder->ref->GetState(callee, isConstructCall, arguments, argumentCount, nullptr);
    }
}

void CALLBACK EnqueueCallback(JsProjectionCallback jsCallback, JsProjectionCallbackContext jsContext, void *callbackState)
{
    jsCallback(jsContext);
}

JsErrorCode JScriptRuntime::CreateHostContext(JsRuntimeHandle runtime, JsContextRef *context)
{
    //
    // Create the context. Note that if we had wanted to start debugging from the very
    // beginning, we would have passed in an IDebugApplication pointer here.
    //

    IfFailRet(JsCreateContext(runtime, context));

    //
    // Now set the execution context as being the current one on this thread.
    //

    IfFailRet(JsSetCurrentContext(*context));

    //
    // Create the host object the script will use.
    //

    IfFailRet(JsCreateObject(&m_hostObject));

    //
    // Get the global object
    //

    JsValueRef globalObject;
    IfFailRet(JsGetGlobalObject(&globalObject));

    //
    // Get the name of the property ("host") that we're going to set on the global object.
    //

    JsPropertyIdRef hostPropertyId;
    IfFailRet(JsGetPropertyIdFromName(L"host", &hostPropertyId));

    //
    // Set the property.
    //
    IfFailRet(JsSetProperty(globalObject, hostPropertyId, m_hostObject, true));

    std::shared_ptr<JScriptRuntimeNativeHolder> echo = std::make_shared<JScriptRuntimeNativeHolder>();
    echo->ref = this;
    echo->method = 0;
    m_holders.push_back(echo);
    IfFailRet(DefineHostCallback(m_hostObject, L"echo", Shim, echo.get()));

    std::shared_ptr<JScriptRuntimeNativeHolder> setState = std::make_shared<JScriptRuntimeNativeHolder>();
    setState->ref = this;
    setState->method = 3;
    m_holders.push_back(setState);
    IfFailRet(DefineHostCallback(m_hostObject, L"setState", Shim, setState.get()));

    std::shared_ptr<JScriptRuntimeNativeHolder> getState = std::make_shared<JScriptRuntimeNativeHolder>();
    getState->ref = this;
    getState->method = 4;
    m_holders.push_back(getState);
    IfFailRet(DefineHostCallback(m_hostObject, L"getState", Shim, getState.get()));

    IfFailRet(DefineHostCallback(m_hostObject, L"runScript", RunScript, nullptr));

    // Set/ClearInterval 
    //
    std::shared_ptr<JScriptRuntimeNativeHolder> setInterval = std::make_shared<JScriptRuntimeNativeHolder>();
    setInterval->ref = this;
    setInterval->method = 1;
    m_holders.push_back(setInterval);
    IfFailRet(DefineHostCallback(globalObject, L"setInterval", Shim, setInterval.get()));
    std::shared_ptr<JScriptRuntimeNativeHolder> clearInterval = std::make_shared<JScriptRuntimeNativeHolder>();
    clearInterval->ref = this;
    clearInterval->method = 3;
    m_holders.push_back(clearInterval);
    IfFailRet(DefineHostCallback(globalObject, L"clearInterval", Shim, clearInterval.get()));

    //
    // Clean up the current execution context.
    //

    IfFailRet(JsSetCurrentContext(JS_INVALID_REFERENCE));

    return JsNoError;
}

// UNDONE: this seems to be calling multiple times, the implementation seems kludgy, generally this feels wrong...
//  ... but, it's good enough for the demo for now
//
JsValueRef CALLBACK JScriptRuntime::SetInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    auto func = arguments[1];
    double interval;
    JsNumberToDouble(arguments[2], &interval);

    int timerId = (m_timerCounter++);

    auto timer = m_timers[timerId] = ref new DispatcherTimer();
    m_timerHandlers[timerId] = func;
    JsAddRef(func, nullptr);

    Windows::Foundation::TimeSpan t;
    t.Duration = (long long)(interval * 10000L);
    timer->Interval = t;

    WeakReference wr(this);

    timer->Tick += ref new Windows::Foundation::EventHandler<Platform::Object ^>(
        [wr, timerId](Platform::Object ^sender, Platform::Object ^args) {
            JScriptRuntime^ c = wr.Resolve<JScriptRuntime>();
            if (c != nullptr)
            {
                c->TriggerTimer(timerId);
            }
            else
            {
                // Inform the event that this handler should be removed
                // from the subscriber list
                throw ref new DisconnectedException();
            }

        });
    timer->Start();

    JsValueRef value;
    JsIntToNumber(timerId, &value);
    return value;
}
JsValueRef CALLBACK JScriptRuntime::ClearInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    int timerId;
    JsNumberToInt(arguments[1], &timerId);

    auto timerHandler = m_timerHandlers.find(timerId);
    auto timer = m_timers.find(timerId);
    if (timerHandler != m_timerHandlers.end()) {
        JsRelease(timerHandler->second, nullptr);
        m_timerHandlers.erase(timerHandler);
    }
    if (timer != m_timers.end()) {
        timer->second->Stop();
        m_timers.erase(timer);
    }

    JsValueRef value;
    JsBoolToBoolean(true, &value);
    return value;
}
void JScriptRuntime::TriggerTimer(int id)
{
    auto timerHandler = m_timerHandlers.find(id);
    if (timerHandler != m_timerHandlers.end())
    {
        auto func = timerHandler->second;
        JsValueRef ret;
        JsCallFunction(func, nullptr, 0, &ret);
    }
}

JsValueRef CALLBACK JScriptRuntime::SetState(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    m_state->AssignJsValue(arguments[1]);
    return JS_INVALID_REFERENCE;
}
JsValueRef CALLBACK JScriptRuntime::GetState(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    return m_state->ToJsValue();
}
JsValueRef CALLBACK JScriptRuntime::Echo(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
    for (unsigned int index = 1; index < argumentCount; index++)
    {
        if (index > 1)
        {
            EchoNotify(L" ");
        }

        JsValueRef stringValue;
        IfFailThrow(JsConvertValueToString(arguments[index], &stringValue), L"invalid argument");

        const wchar_t *string;
        size_t length;
        IfFailThrow(JsStringToPointer(stringValue, &string, &length), L"invalid argument");

        EchoNotify(ref new Platform::String(string));
    }

    EchoNotify(L"\n");

    return JS_INVALID_REFERENCE;
}
Platform::String^ JScriptRuntime::GetScriptException()
{
    return ::GetScriptException();
}
void JScriptRuntime::AddHostObject(Platform::String^ name, Platform::Object^ value)
{
    JsErrorCode c;
    IfFailThrowNoRet(c = DefineHostInspectable(m_hostObject, name->Data(), reinterpret_cast<IInspectable*>(value)), L"failed to add object");
}
void JScriptRuntime::AddGlobalObject(Platform::String^ name, Platform::Object^ value)
{
    JsErrorCode c;
    JsValueRef globalObject;
    IfFailThrowNoRet(c = JsGetGlobalObject(&globalObject), L"can't get global");
    IfFailThrowNoRet(c = DefineHostInspectable(globalObject, name->Data(), reinterpret_cast<IInspectable*>(value)), L"failed to add object");
}
void JScriptRuntime::AddWinRTNamespace(Platform::String^ name)
{
    JsErrorCode c;
    IfFailThrowNoRet(c = JsProjectWinRTNamespace(name->Data()), L"failed to add namespace");
    // IfFailThrowNoRet(c = JsSetProjectionEnqueueCallback(EnqueueCallback, nullptr), L"failed to register callback");
}
void JScriptRuntime::StartDebugging()
{
    JsErrorCode c;
    IfFailThrowNoRet(c = JsStartDebugging(), L"Failed to start debugging");
}
void JScriptRuntime::SetActive()
{
    JsErrorCode c;
    IfFailThrowNoRet(c = JsSetCurrentContext(m_context), L"Failed to set the current context");
}
void JScriptRuntime::ClearActive()
{
    JsErrorCode c;
    IfFailThrowNoRet(c = JsSetCurrentContext(JS_INVALID_REFERENCE), L"Failed to clear the current context");
}
void JScriptRuntime::ClearTimers()
{
    for (auto it = m_timerHandlers.begin(); it != m_timerHandlers.end(); it++)
    {
        JsRelease(it->second, nullptr);
    }
    m_timerHandlers.clear();

    for (auto it = m_timers.begin(); it != m_timers.end(); it++)
    {
        it->second->Stop();
    }
    m_timers.clear();
}

int JScriptRuntime::Eval(String^ script)
{
    auto strscript = script->Data();
    return JScriptEval(m_runtime, strscript);
}

JScriptRuntime::~JScriptRuntime()
{
    IfFailError(JsDisposeRuntime(m_runtime), L"failed to cleanup runtime.");
    m_holders.clear();
error:
    ;
}

