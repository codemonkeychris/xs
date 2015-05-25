/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ a: 2 });
        host.setState({ a: 3 });
    };

    function render() {
        return (
            React.createElement(Xaml.TextBlock, {text: JSON.stringify(host.getState()), fontSize: "36", margin: "10,10,10,10"})
        );
    }
    App.render = render;
})(App || (App = {}));
