/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function setInitialState() {
        var initialText = JSON.stringify({ type: 'TextBlock', text: "hello world" });
        host.setState({
            text: initialText,
            content: JSON.parse(initialText)
        });
    }
    App.setInitialState = setInitialState;
    ;
    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }
    function refreshClicked(sender, e) {
        host.setState({ content: JSON.parse(host.getState().text) });
    }
    function MultiLineTextBox() {
        return React.createElement(Xaml.TextBox, {"scrollViewer$horizontalScrollBarVisibility": 'Auto', "scrollViewer$verticalScrollBarVisibility": 'Auto', "acceptsReturn": true, "textWrapping": 'Wrap', "horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch'});
    }
    function render() {
        return (React.createElement(Xaml.Grid, {"horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch', "rows": ['auto', '*', 'auto'], "columns": ['*', 'auto', '*']}, React.createElement(MultiLineTextBox, {"grid$row": 1, "grid$column": 0, "fontFamily": 'Consolas', "fontSize": 16, "onTextChanged": textChanged, "text": host.getState().text}), React.createElement(Xaml.Button, {"grid$row": 1, "grid$column": 1, "content": 'refresh', "onClick": refreshClicked}), React.createElement(Xaml.ContentControl, {"grid$row": 1, "grid$column": 2, "content": host.getState().text})));
    }
    App.render = render;
})(App || (App = {}));
