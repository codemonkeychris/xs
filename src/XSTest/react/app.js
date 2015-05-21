/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function render() {
        return (
            React.createElement(Xaml.RelativePanel, {name: "rel1"}, 
                React.createElement(Xaml.Button, {margin: "5,5,5,5", name: "b1"}, 
                    "One"
                ), 
                React.createElement(Xaml.Button, {margin: "5,5,5,5", name: "b2", relative$below: "b1"}, 
                    "Two"
                ), 
                React.createElement(Xaml.Button, {margin: "5,5,5,5", name: "b3", relative$below: "b1", relative$rightOf: "b2"}, 
                    "Three"
                )
            )
        );
    }
    App.render = render;
})(App || (App = {}));
