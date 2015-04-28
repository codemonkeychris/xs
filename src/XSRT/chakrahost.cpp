#include "pch.h"

using namespace std;

//
// Source context counter.
//

unsigned currentSourceContext = 0;

//
// This "throws" an exception in the Chakra space. Useful routine for callbacks
// that need to throw a JS error to indicate failure.
//

void ThrowException(wstring errorString)
{
	// We ignore error since we're already in an error state.
	JsValueRef errorValue;
	JsValueRef errorObject;
	JsPointerToString(errorString.c_str(), errorString.length(), &errorValue);
	JsCreateError(errorValue, &errorObject);
	JsSetException(errorObject);
}

//
// Helper to load a script from disk.
//

wstring LoadScript(wstring fileName)
{
	FILE *file;
	if (_wfopen_s(&file, fileName.c_str(), L"rb"))
	{
		fwprintf(stderr, L"chakrahost: unable to open file: %s.\n", fileName.c_str());
		return wstring();
	}

	unsigned int current = ftell(file);
	fseek(file, 0, SEEK_END);
	unsigned int end = ftell(file);
	fseek(file, current, SEEK_SET);
	unsigned int lengthBytes = end - current;
	char *rawBytes = (char *)calloc(lengthBytes + 1, sizeof(char));

	if (rawBytes == nullptr)
	{
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	fread((void *)rawBytes, sizeof(char), lengthBytes, file);

	wchar_t *contents = (wchar_t *)calloc(lengthBytes + 1, sizeof(wchar_t));
	if (contents == nullptr)
	{
		free(rawBytes);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	if (MultiByteToWideChar(CP_UTF8, 0, rawBytes, lengthBytes + 1, contents, lengthBytes + 1) == 0)
	{
		free(rawBytes);
		free(contents);
		fwprintf(stderr, L"chakrahost: fatal error.\n");
		return wstring();
	}

	wstring result = contents;
	free(rawBytes);
	free(contents);
	return result;
}

//
// Callback to echo something to the command-line.
//


//
// Callback to load a script and run it.
//

JsValueRef CALLBACK RunScript(JsValueRef callee, bool isConstructCall, JsValueRef *arguments, unsigned short argumentCount, void *callbackState)
{
	JsValueRef result = JS_INVALID_REFERENCE;

	if (argumentCount < 2)
	{
		ThrowException(L"not enough arguments");
		return result;
	}

	//
	// Convert filename.
	//
	const wchar_t *filename;
	size_t length;

	IfFailThrow(JsStringToPointer(arguments[1], &filename, &length), L"invalid filename argument");

	//
	// Load the script from the disk.
	//

	wstring script = LoadScript(filename);
	if (script.empty())
	{
		ThrowException(L"invalid script");
		return result;
	}

	//
	// Run the script.
	//

	IfFailThrow(JsRunScript(script.c_str(), currentSourceContext++, filename, &result), L"failed to run script.");

	return result;
}

//
// Helper to define a host callback method on the global host object.
//

JsErrorCode DefineHostCallback(JsValueRef globalObject, const wchar_t *callbackName, JsNativeFunction callback, void *callbackState)
{
	//
	// Get property ID.
	//

	JsPropertyIdRef propertyId;
	IfFailRet(JsGetPropertyIdFromName(callbackName, &propertyId));

	//
	// Create a function
	//

	JsValueRef function;
	IfFailRet(JsCreateFunction(callback, callbackState, &function));

	//
	// Set the property
	//

	IfFailRet(JsSetProperty(globalObject, propertyId, function, true));

	return JsNoError;
}

//
// Creates a host execution context and sets up the host object in it.
//


//
// Print out a script exception.
//

JsErrorCode PrintScriptException()
{
	//
	// Get script exception.
	//

	JsValueRef exception;
	IfFailRet(JsGetAndClearException(&exception));

	//
	// Get message.
	//

	JsPropertyIdRef messageName;
	IfFailRet(JsGetPropertyIdFromName(L"message", &messageName));

	JsValueRef messageValue;
	IfFailRet(JsGetProperty(exception, messageName, &messageValue));

	const wchar_t *message;
	size_t length;
	IfFailRet(JsStringToPointer(messageValue, &message, &length));

	fwprintf(stderr, L"chakrahost: exception: %s\n", message);

	return JsNoError;
}

int JScriptEval(
	JsRuntimeHandle runtime,
	JsContextRef context,
	wstring script)
{
	int returnValue = EXIT_FAILURE;

	try
	{
		//
		// Now set the execution context as being the current one on this thread.
		//

		IfFailError(JsSetCurrentContext(context), L"failed to set current context.");


		if (script.empty())
		{
			goto error;
		}

		//
		// Run the script.
		//

		JsValueRef result;
		JsErrorCode errorCode = JsRunScript(script.c_str(), currentSourceContext++, L"." /*source URL*/, &result);

		if (errorCode == JsErrorScriptException)
		{
			IfFailError(PrintScriptException(), L"failed to print exception");
			return EXIT_FAILURE;
		}
		else
		{
			IfFailError(errorCode, L"failed to run script.");
		}

		//
		// Convert the return value.
		//

		JsValueRef numberResult;
		double doubleResult;
		IfFailError(JsConvertValueToNumber(result, &numberResult), L"failed to convert return value.");
		IfFailError(JsNumberToDouble(numberResult, &doubleResult), L"failed to convert return value.");
		returnValue = (int)doubleResult;

		//
		// Clean up the current execution context.
		//

		IfFailError(JsSetCurrentContext(JS_INVALID_REFERENCE), L"failed to cleanup current context.");
	}
	catch (...)
	{
		fwprintf(stderr, L"chakrahost: fatal error: internal error.\n");
	}

error:
	return returnValue;
}
