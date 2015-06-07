/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return React.createElement(Xaml.Button, {margin: "5,5,5,5"});
    }

    function render() {
        return (
            React.createElement(Xaml.StackPanel, {horizontalAlignment: "Stretch", verticalAlignment: "Stretch"}, 
                React.createElement(Xaml.Button, {name: "b1", content: "hello"}), 
                React.createElement(Xaml.ProgressBar, {name: "p1", minimum: "0", maximum: "10", value: "5"}), 
                React.createElement(Xaml.Slider, {name: "s1", minimum: "0", maximum: "10", value: "5"})
            )
        );
    }
    App.render = render;
})(App || (App = {}));
