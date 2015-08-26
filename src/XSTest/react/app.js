/// <reference path='../../xsrt2/xsrt.d.ts' />
function circle(radius) {
    return React.createElement(Xaml.Ellipse, {"width": radius, "height": radius, "fill": "Black"});
}
function hstack(children) {
    return React.createElement(Xaml.StackPanel, {"orientation": 'Horizontal'}, children);
}
function vstack(children) {
    return React.createElement(Xaml.StackPanel, {"orientation": 'Vertical'}, children);
}
function range(min, max) {
    var result = [];
    for (var i = min; i < max; i++) {
        result.push(i);
    }
    return result;
}
var App;
(function (App) {
    function setInitialState() {
        var helloWorld = doEval("t = " + JSON.stringify({ type: 'TextBlock', text: "hello world" }));
        var sample = doEval("hstack(range(1,5).map(function(i) { return i * 10; }).map(circle))");
        host.setState({
            text: "",
            history: [
                helloWorld,
                { inputText: "1+1", result: 1 + 1 },
                sample
            ]
        });
    }
    App.setInitialState = setInitialState;
    ;
    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }
    function evalClicked(sender, e) {
        var entry = doEval(host.getState().text);
        var history = host.getState().history;
        history.push(entry);
        host.setState({ history: history, text: '' });
    }
    function doEval(input) {
        var entry;
        var result;
        try {
            result = eval(input);
        }
        catch (e) {
            result = "" + e;
        }
        try {
            entry = {
                inputText: input,
                result: JSON.parse(JSON.stringify(result))
            };
        }
        catch (e) {
            entry = { inputText: input, result: result };
        }
        return entry;
    }
    function renderEntry(e) {
        return (React.createElement(Xaml.StackPanel, null, React.createElement(Xaml.TextBlock, {"text": ">" + e.inputText}), React.createElement(Xaml.ContentControl, {"content": e.result})));
    }
    function EntryBox() {
        return React.createElement(Xaml.TextBox, {"fontFamily": 'Consolas', "fontSize": 14, "width": 450, "horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch'});
    }
    function render() {
        return (React.createElement(Xaml.ScrollViewer, null, React.createElement(Xaml.StackPanel, {"horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch'}, React.createElement(Xaml.StackPanel, null, host.getState().history.map(renderEntry)), React.createElement(Xaml.StackPanel, {"orientation": 'Horizontal'}, React.createElement(EntryBox, {"onTextChanged": textChanged, "text": host.getState().text}), React.createElement(Xaml.Button, {"content": 'eval', "onClick": evalClicked})))));
    }
    App.render = render;
})(App || (App = {}));
