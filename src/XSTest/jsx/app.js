/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function render() {
        return (
            <Xaml.RelativePanel name='rel1'>
                <Xaml.Button margin='5,5,5,5' name='b1'>
                    One
                </Xaml.Button>
                <Xaml.Button margin='5,5,5,5' name='b2' relative$below='b1'>
                    Two
                </Xaml.Button>
                <Xaml.Button margin='5,5,5,5' name='b3' relative$below='b1' relative$rightOf='b2'>
                    Three
                </Xaml.Button>
            </Xaml.RelativePanel>
        );
    }
    App.render = render;
})(App || (App = {}));
