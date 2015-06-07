/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return <Xaml.Button margin='5,5,5,5' />;
    }

    function render() {
        return (
            <Xaml.StackPanel horizontalAlignment='Stretch' verticalAlignment='Stretch'>
                <Xaml.Button name='b1' content='hello' />
                <Xaml.ProgressBar name='p1' minimum='0' maximum='10' value='5' />
                <Xaml.Slider name='s1' minimum='0' maximum='10' value='5' />
            </Xaml.StackPanel>
        );
    }
    App.render = render;
})(App || (App = {}));
