#pragma once
namespace JSRT
{
    public ref class JScriptValueMarshaller sealed
    {
        JsValueRef m_valueRoot;
        uint64 m_version;
    internal:
        void FromJsValue(JsValueRef& value);
        JsValueRef ToJsValue();
        void AssignJsValue(JsValueRef& value);
    public:
        JScriptValueMarshaller();

        uint64 GetValueVersion() { return m_version; }
        void FromObject(Platform::Object^ value);
        Platform::Object^ ToObject();
    };

    Platform::Object^ HandleAny(JsValueRef value);
    Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ HandleRecord(JsValueRef value);
    Windows::Foundation::Collections::IVector<Platform::Object^>^ HandleArray(JsValueRef value);
    Platform::Object^ HandlePrimitive(JsValueRef value);


    JsValueRef InvertHandleAny(Platform::Object^ value);
    JsValueRef InvertHandleRecord(Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ value);
    JsValueRef InvertHandleArray(Windows::Foundation::Collections::IVector<Platform::Object^>^ value);
    JsValueRef InvertHandlePrimitive(Platform::Object^ value);

}