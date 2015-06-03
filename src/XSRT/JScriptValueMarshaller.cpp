#include "pch.h"
#include "JScriptValueMarshaller.h"
// #define DEBUG_WRITE(x) OutputDebugString(x);
// #define DEBUG_WRITELN(x) { OutputDebugString(x); OutputDebugString(L"\n"); }
// #define DEBUG
#define DEBUG_WRITE(x) ;
#define DEBUG_WRITELN(x) ;

using namespace JSRT;

JScriptValueMarshaller::JScriptValueMarshaller()
{
    m_version++;
    m_valueRoot = JS_INVALID_REFERENCE;
}

void JScriptValueMarshaller::FromJsValue(JsValueRef& value)
{
    m_version++;
    if (m_valueRoot != JS_INVALID_REFERENCE)
    {
        JsRelease(m_valueRoot, nullptr);
        m_valueRoot = JS_INVALID_REFERENCE;
    }
    m_valueRoot = value;
    JsAddRef(m_valueRoot, nullptr);
}
JsValueRef JScriptValueMarshaller::ToJsValue()
{
    return m_valueRoot;
}
JsValueRef MergeObjectValue(JsValueRef target, JsValueRef source)
{
    JsValueRef objSource, objTarget;
    JsConvertValueToObject(source, &objSource);
    JsConvertValueToObject(target, &objTarget);

    // Object.keys... 
    //
    JsValueRef propertyNames;
    JsGetOwnPropertyNames(objSource, &propertyNames);

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

        DEBUG_WRITE(L"merge name:");
        DEBUG_WRITELN(strName)
        JsValueRef value;
        JsGetProperty(objSource, nameId, &value);
#ifdef DEBUG
        JsValueType vt(JsValueType::JsUndefined);
        if (vt == JsValueType::JsString)
        {
            JsStringToPointer(value, &strName, &strLen);
            DEBUG_WRITE(L"merge value:");
            DEBUG_WRITELN(strName);
        }
#endif
        JsSetProperty(objTarget, nameId, value, true);
    }

    return objTarget;
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
    
    JsValueRef newValue = MergeObjectValue(m_valueRoot, value);
    this->FromJsValue(newValue);
}

Platform::Object^ JSRT::HandleAny(JsValueRef value)
{
    JsValueType vt;
    JsGetValueType(value, &vt);
    switch (vt)
    {
    case JsValueType::JsArray:
        return JSRT::HandleArray(value);
    case JsValueType::JsObject:
        return JSRT::HandleRecord(value);
    default:
        return JSRT::HandlePrimitive(value);
    }
}

Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ JSRT::HandleRecord(JsValueRef value)
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

        auto child = JSRT::HandleAny(v);

        map->Insert(ref new Platform::String(strName), child);
    }

    return map;
}
Windows::Foundation::Collections::IVector<Platform::Object^>^ JSRT::HandleArray(JsValueRef value)
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

        auto child = JSRT::HandleAny(v);

        a->Append(child);
    }

    return a;
}
Platform::Object^ JSRT::HandlePrimitive(JsValueRef value)
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

JsValueRef JSRT::InvertHandleAny(Platform::Object^ value)
{
    auto map = dynamic_cast<Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^>(value);
    auto a = dynamic_cast<Windows::Foundation::Collections::IVector<Platform::Object^>^>(value);
    if (map != nullptr)
    {
        return JSRT::InvertHandleRecord(map);
    }
    else if (a != nullptr)
    {
        return JSRT::InvertHandleArray(a);
    }
    else
    {
        return JSRT::InvertHandlePrimitive(value);
    }
    return JS_INVALID_REFERENCE;
}
JsValueRef JSRT::InvertHandleRecord(Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^ value)
{
    JsValueRef result;
    JsCreateObject(&result);

    auto it = value->First();
    while (it->HasCurrent)
    {
        auto key = it->Current->Key;
        auto v = JSRT::InvertHandleAny(it->Current->Value);
        DEBUG_WRITE(L"prop:");
        DEBUG_WRITELN(key->Data());

        JsPropertyIdRef prop;
        JsGetPropertyIdFromName(key->Data(), &prop);

        JsSetProperty(result, prop, v, true);
        it->MoveNext();
    }

    return result;
}
JsValueRef JSRT::InvertHandleArray(Windows::Foundation::Collections::IVector<Platform::Object^>^ value)
{
    return JS_INVALID_REFERENCE;
}
JsValueRef JSRT::InvertHandlePrimitive(Platform::Object^ value)
{
    JsValueRef v = JS_INVALID_REFERENCE;
    DEBUG_WRITE(L"value:");

    auto d = dynamic_cast<Platform::Box<double>^>(value);
    if (d != nullptr)
    {
        JsDoubleToNumber((double)d, &v);
        DEBUG_WRITELN(L"number");
        return v;
    }
    auto b = dynamic_cast<Platform::Box<bool>^>(value);
    if (b != nullptr)
    {
        JsBoolToBoolean((bool)b, &v);
        DEBUG_WRITELN(((bool)b) ? L"true" : L"false");
        return v;
    }
    auto str = dynamic_cast<Platform::String^>(value);
    if (str != nullptr)
    {
        JsPointerToString(str->Data(), str->Length(), &v);
        DEBUG_WRITELN(str->Data());
        return v;
    }
    return v;
}

void JScriptValueMarshaller::FromObject(Platform::Object^ value)
{
    m_version++;

    JsValueRef newValue = JSRT::InvertHandleAny(value);
    this->FromJsValue(newValue);
}
