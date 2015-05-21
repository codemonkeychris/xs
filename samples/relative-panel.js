/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return <Xaml.Button margin='5,5,5,5' />;
    }

    function render() {
        return (
            <Xaml.RelativePanel name='rel1' childrenTransitions={[<Xaml.RepositionThemeTransition />]}>
                <MyButton name='b1'>
                    One
                </MyButton>
                <MyButton name='b2' relative$below='b1'>
                    Two
                </MyButton>
                <MyButton name='b3' relative$below='b1' relative$rightOf='b2'>
                    Three
                </MyButton>
                <MyButton name='b4' relative$below='b3' relative$rightOf='b3'>
                    Four
                </MyButton>
            </Xaml.RelativePanel>
        );
    }
    App.render = render;
})(App || (App = {}));
