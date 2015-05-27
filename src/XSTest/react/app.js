/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            text: "test"
        });
    };

    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }

    function render() {

        return (
            React.createElement(Xaml.Grid, {
                rows: ['auto', '*'], 
                columns: ['*']}, 
                
                React.createElement(Xaml.TextBlock, {name: "label1", grid$row: "0"}, "count:" + host.getState().text.length), 
                React.createElement(Xaml.TextBox, {
                    grid$row: "1", 
                    grid$column: "0", 
                    fontFamily: "Consolas", 
                    fontSize: "20", 
                    onTextChanged: textChanged, 
                    text: host.getState().text})
            )
        );
    }
    App.render = render;
})(App || (App = {}));

