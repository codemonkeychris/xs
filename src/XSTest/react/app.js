/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return React.createElement(Xaml.Button, {margin: "5,5,5,5"});
    }

    function render() {
        return (
            React.createElement(Xaml.RelativePanel, {name: "rel1", childrenTransitions: [React.createElement(Xaml.RepositionThemeTransition, null)]}, 
                React.createElement(MyButton, {name: "b1"}, 
                    "One"
                ), 
                React.createElement(MyButton, {name: "b2", relative$below: "b1"}, 
                    "Two"
                ), 
                React.createElement(MyButton, {name: "b3", relative$below: "b1", relative$rightOf: "b2"}, 
                    "Three"
                ), 
                React.createElement(MyButton, {name: "b4", relative$below: "b3", relative$rightOf: "b3"}, 
                    "Four this is fun"
                )
            )
        );
    }
    App.render = render;
})(App || (App = {}));
