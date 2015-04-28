#pragma once
#define USE_EDGEMODE_JSRT 1
#include "jsrt.h"


int JScriptEval(
	JsRuntimeHandle runtime,
	JsContextRef context,
	std::wstring script);

JsErrorCode DefineHostCallback(JsValueRef globalObject, const wchar_t *callbackName, JsNativeFunction callback, void *callbackState);

JsValueRef CALLBACK RunScript(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState);

void ThrowException(std::wstring errorString);

#define IfFailError(v, e) \
    { \
    JsErrorCode error = (v); \
if (error != JsNoError) \
        { \
        fwprintf(stderr, L"chakrahost: fatal error: %s.\n", (e)); \
        goto error; \
        } \
    }

#define IfFailRet(v) \
    { \
    JsErrorCode error = (v); \
if (error != JsNoError) \
        { \
        return error; \
        } \
    }

#define IfFailThrow(v, e) \
    { \
    JsErrorCode error = (v); \
if (error != JsNoError) \
        { \
        ThrowException((e)); \
        return JS_INVALID_REFERENCE; \
        } \
    }

#define IfComFailError(v) \
    { \
    hr = (v); \
if (FAILED(hr)) \
        { \
        goto error; \
        } \
    }

#define IfComFailRet(v) \
    { \
    HRESULT hr = (v); \
if (FAILED(hr)) \
        { \
        return hr; \
        } \
    }

