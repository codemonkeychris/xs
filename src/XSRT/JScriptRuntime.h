#pragma once

namespace XSRT
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

		JsErrorCode CreateHostContext(JsRuntimeHandle runtime, JsContextRef *context);
		~JScriptRuntime();
	internal:
		JsValueRef CALLBACK Echo(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);
	public:
		JScriptRuntime();
		void SetActive();
		void ClearActive();
		void AddWinRTNamespace(Platform::String^ name);
		void AddHostObject(Platform::String^ name, Platform::Object^ value);
		int Eval(Platform::String^ script);
		event EchoEventHandler^ EchoNotify;
	};
}