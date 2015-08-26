#include "pch.h"
#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace JSRT;

namespace JSRTUnitTests
{
    TEST_CLASS(UnitTest1)
    {
    public:
        TEST_METHOD(EvalMath)
        {
            JScriptRuntime^ rt = ref new JScriptRuntime();
            rt->SetActive();
            rt->AddWinRTNamespace(L"Windows");
            auto result = rt->Eval(L"1+1");
            rt->ClearActive();

            auto n = dynamic_cast<Platform::IBox<double>^>(result);
            auto doubleResult = n->Value;
            Assert::AreEqual(2.0, doubleResult, L"1+1 should always be 2");
        }

        TEST_METHOD(ReturnStruct)
        {
            JScriptRuntime^ rt = ref new JScriptRuntime();
            rt->SetActive();
            rt->AddWinRTNamespace(L"Windows");
            auto result = rt->Eval(L"t={x:10}");
            rt->ClearActive();

            auto map = dynamic_cast<Windows::Foundation::Collections::IMap<Platform::String^, Platform::Object^>^>(result);
            auto iterator = map->First();
            Assert::IsTrue(iterator->HasCurrent, L"Should have at least 1 item in the result");
            Assert::AreEqual(L"x", iterator->Current->Key, L"Name should be 'x'");
            auto value = iterator->Current->Value;
            auto n = dynamic_cast<Platform::IBox<double>^>(value);
            auto doubleResult = n->Value;
            Assert::AreEqual(10.0, doubleResult, L"'x' should be 10.0");
        }

    };
}