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

    function MultiLineTextBox() {
        return React.createElement(Xaml.TextBox, {
            scrollViewer$horizontalScrollBarVisibility: "Auto", 
            scrollViewer$verticalScrollBarVisibility: "Auto", 
            acceptsReturn: true, 
            textWrapping: "Wrap", 
            horizontalAlignment: "Stretch", 
            verticalAlignment: "Stretch"})
    }

    function render() {

        return (
            React.createElement(Xaml.Grid, {
                horizontalAlignment: "Stretch", 
                verticalAlignment: "Stretch", 
                rows: ['auto', '*'], 
                columns: ['*']}, 
                
                React.createElement(Xaml.TextBlock, {name: "label1", grid$row: "0"}, "count:" + host.getState().text.length), 
                React.createElement(MultiLineTextBox, {
                    grid$row: "1", 
                    grid$column: "0", 
                    fontFamily: "Consolas", 
                    fontSize: "14", 
                    onTextChanged: textChanged, 
                    text: host.getState().text})
            )
        );
    }
    App.render = render;
})(App || (App = {}));

