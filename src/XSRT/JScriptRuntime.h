#pragma once

namespace JSRT
{
    ref class JScriptRuntime;

    class JScriptRuntimeNativeHolder
    {
    public:
        JScriptRuntime^ ref;
        int method;
    };

    public delegate void EchoEventHandler(Platform::String^ message);


    public ref class JScriptRuntime sealed
    {
    private:
        JsRuntimeHandle m_runtime;
        JsContextRef m_context;
        JsValueRef m_hostObject;
        std::list<std::shared_ptr<JScriptRuntimeNativeHolder>> m_holders;
        int m_timerCounter;
        std::map<int, Windows::UI::Xaml::DispatcherTimer^> m_timers;
        std::map<int, JsValueRef> m_timerHandlers;
        JScriptValueMarshaller^ m_state;

        JsErrorCode CreateHostContext(JsRuntimeHandle runtime, JsContextRef *context);
        ~JScriptRuntime();
    internal:
        JsValueRef CALLBACK Echo(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
        JsValueRef CALLBACK SetInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
        JsValueRef CALLBACK ClearInterval(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
        JsValueRef CALLBACK SetState(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
        JsValueRef CALLBACK GetState(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
    public:
        JScriptRuntime();
        void SetActive();
        void StartDebugging();
        void ClearActive();
        void ClearTimers();
        uint64 GetStateVersion() { return m_state->GetValueVersion(); }
        Platform::Object^ SaveState() { return m_state->ToObject(); }
        void LoadState(Platform::Object^ value) { m_state->FromObject(value); }
        Platform::String^ GetScriptException();
        void AddWinRTNamespace(Platform::String^ name);
        void AddHostObject(Platform::String^ name, Platform::Object^ value);
        void AddGlobalObject(Platform::String^ name, Platform::Object^ value);
        int Eval(Platform::String^ script);
        event EchoEventHandler^ EchoNotify;
        void TriggerTimer(int id);
    };
}