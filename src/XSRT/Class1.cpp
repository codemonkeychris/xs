#include "pch.h"
#include "Class1.h"

using namespace XSRT;
using namespace Platform;

JScriptRuntime::JScriptRuntime()
{
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
	}
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

	IfFailRet(DefineHostCallback(m_hostObject, L"runScript", RunScript, nullptr));

	//
	// Clean up the current execution context.
	//

	IfFailRet(JsSetCurrentContext(JS_INVALID_REFERENCE));

	return JsNoError;
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

int JScriptRuntime::Eval(String^ script)
{
	auto strscript = script->Data();
	return JScriptEval(m_runtime, m_context, strscript);
}

JScriptRuntime::~JScriptRuntime()
{
	IfFailError(JsDisposeRuntime(m_runtime), L"failed to cleanup runtime.");
	m_holders.clear();
error:
	;
}
