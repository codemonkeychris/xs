#include "pch.h"
#include "JScriptValueMarshaller.h"

using namespace XSRT;

JScriptValueMarshaller::JScriptValueMarshaller()
{
    m_version++;
    m_valueRoot = JS_INVALID_REFERENCE;
}

void JScriptValueMarshaller::FromJsValue(JsValueRef& value)
{
    m_version++;
    m_valueRoot = value;
}
JsValueRef JScriptValueMarshaller::ToJsValue()
{
    return m_valueRoot;
}
JsValueRef MergeObjectValue(JsValueRef target, JsValueRef source)
{
    JsValueRef obj;
    JsConvertValueToObject(source, &obj);

    // Object.keys... 
    //
    JsValueRef propertyNames;
    JsGetOwnPropertyNames(obj, &propertyNames);

    // for(int i=0; i<keys.length; i++) {
    //
    JsPropertyIdRef lengthId;
    JsGetPropertyIdFromName(L"length", &lengthId);
    JsValueRef length;
    JsGetProperty(propertyNames, lengthId, &length);
    int n;
    JsNumberToInt(length, &n);

    for (int i = 0; i < n; i++)
    {
        // name = keys[i]
        //
        JsValueRef index, name;
        JsIntToNumber(i, &index);
        JsGetIndexedProperty(propertyNames, index, &name);

        // target[name] = source[name]
        //
        const wchar_t *strName;
        size_t strLen;
        JsStringToPointer(name, &strName, &strLen);
        JsPropertyIdRef nameId;
        JsGetPropertyIdFromName(strName, &nameId);


        JsValueRef value;
        JsGetProperty(source, nameId, &value);
        JsSetProperty(target, nameId, value, true);
    }

    return target;
}

void JScriptValueMarshaller::AssignJsValue(JsValueRef& value)
{
    m_version++;
    JsValueType vt;
    JsGetValueType(value, &vt);

    if (m_valueRoot == JS_INVALID_REFERENCE || vt != JsValueType::JsObject) 
    { 
        this->FromJsValue(value); 
        return; 
    }
    
    m_valueRoot = MergeObjectValue(m_valueRoot, value);
}

Platform::Object^ XSRT::HandleAny(JsValueRef value)
{
    JsValueType vt;
    JsGetValueType(value, &vt);
    switch (vt)
    {
    case JsValueType::JsArray:
        return XSRT::HandleArray(value);
    case JsValueType::JsObject:
        return XSRT::HandleRecord(value);
    default:
        return XSRT::HandlePrimitive(value);
    }
}

Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ XSRT::HandleRecord(JsValueRef value)
{
    auto map = ref new Platform::Collections::Map<Platform::String^, Platform::Object^>();

    JsValueRef obj;
    JsConvertValueToObject(value, &obj);
    JsValueRef propertyNames;
    JsGetOwnPropertyNames(obj, &propertyNames);
    JsPropertyIdRef lengthId;
    JsGetPropertyIdFromName(L"length", &lengthId);
    JsValueRef length;
    JsGetProperty(propertyNames, lengthId, &length);
    int n;
    JsNumberToInt(length, &n);

    for (int i = 0; i < n; i++)
    {
        JsValueRef index, name;
        JsIntToNumber(i, &index);
        JsGetIndexedProperty(propertyNames, index, &name);

        JsValueRef stringValue;
        JsConvertValueToString(name, &stringValue);

        const wchar_t *strName;
        size_t strLen;
        JsStringToPointer(stringValue, &strName, &strLen);

        JsPropertyIdRef nameId;
        JsGetPropertyIdFromName(strName, &nameId);

        JsValueRef v;
        JsGetProperty(obj, nameId, &v);

        auto child = XSRT::HandleAny(v);

        map->Insert(ref new Platform::String(strName), child);
    }

    return map;
}
Windows::Foundation::Collections::IVector<Platform::Object^>^ XSRT::HandleArray(JsValueRef value)
{
    auto a = ref new Platform::Collections::Vector<Platform::Object^>();

    JsPropertyIdRef lengthId;
    JsGetPropertyIdFromName(L"length", &lengthId);
    JsValueRef length;
    JsGetProperty(value, lengthId, &length);
    int n;
    JsNumberToInt(length, &n);

    for (int i = 0; i < n; i++)
    {
        JsValueRef index, v;
        JsIntToNumber(i, &index);
        JsGetIndexedProperty(value, index, &v);

        auto child = XSRT::HandleAny(v);

        a->Append(child);
    }

    return a;
}
Platform::Object^ XSRT::HandlePrimitive(JsValueRef value)
{
    JsValueType vt;
    JsGetValueType(value, &vt);
    switch (vt)
    {
    case JsValueType::JsNull:
    case JsValueType::JsUndefined:
        // UNDONE
        return nullptr;
    case JsValueType::JsBoolean:
        bool b;
        JsBooleanToBool(value, &b);
        return ref new Platform::Box<bool>(b);
    case JsValueType::JsNumber:
        double d;
        JsNumberToDouble(value, &d);
        return ref new Platform::Box<double>(d);
    case JsValueType::JsString:
        JsValueRef stringValue;
        JsConvertValueToString(value, &stringValue);

        const wchar_t *str;
        size_t len;
        JsStringToPointer(stringValue, &str, &len);
        return ref new Platform::String(str);
    }

    // UNDONE
    return nullptr;
}

Platform::Object^ JScriptValueMarshaller::ToObject()
{
    if (m_valueRoot == JS_INVALID_REFERENCE)
    {
        return nullptr;
    }
    else
    {
        return HandleAny(m_valueRoot);
    }
}

JsValueRef XSRT::InvertHandleAny(Platform::Object^ value)
{
    auto map = dynamic_cast<Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^>(value);
    auto a = dynamic_cast<Windows::Foundation::Collections::IVector<Platform::Object^>^>(value);
    if (map != nullptr)
    {
        return XSRT::InvertHandleRecord(map);
    }
    else if (a != nullptr)
    {
        return XSRT::InvertHandleArray(a);
    }
    else
    {
        return XSRT::InvertHandlePrimitive(value);
    }
    return JS_INVALID_REFERENCE;
}
JsValueRef XSRT::InvertHandleRecord(Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ value)
{
    JsValueRef v;
    JsCreateObject(&v);

    auto it = value->First();
    while (it->HasCurrent)
    {
        auto key = it->Current->Key;
        auto v = XSRT::InvertHandleAny(it->Current->Value);
        
        JsPropertyIdRef prop;
        JsGetPropertyIdFromName(key->Data(), &prop);

        JsSetProperty(v, prop, v, true);
        it->MoveNext();
    }

    return v;
}
JsValueRef XSRT::InvertHandleArray(Windows::Foundation::Collections::IVector<Platform::Object^>^ value)
{
    return JS_INVALID_REFERENCE;
}
JsValueRef XSRT::InvertHandlePrimitive(Platform::Object^ value)
{
    JsValueRef v = JS_INVALID_REFERENCE;

    auto d = dynamic_cast<Platform::Box<double>^>(value);
    if (d != nullptr)
    {
        JsDoubleToNumber((double)d, &v);
        return v;
    }
    auto b = dynamic_cast<Platform::Box<bool>^>(value);
    if (b != nullptr)
    {
        JsBoolToBoolean((bool)b, &v);
        return v;
    }
    auto str = dynamic_cast<Platform::String^>(value);
    if (str != nullptr)
    {
        JsPointerToString(str->Data(), str->Length(), &v);
        return v;
    }
    return v;
}

void JScriptValueMarshaller::FromObject(Platform::Object^ value)
{
    m_version++;
    m_valueRoot = XSRT::InvertHandleAny(value);
}
