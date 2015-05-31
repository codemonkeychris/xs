/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            text: "test"
        });
    };

    var testHandlers = {
        simple_test_1: {
            state: {
                text: "a"
            },
            ready: function(ev) {
                var label1 = ev.root.findName("label1");
                xsrt.assert(label1.text === "1", "should have number '1' in label");
            }
        }
    }
    function testSetup(ev) {
        host.setState(testHandlers[ev.name].state);
    }
    function testReady(ev) {
        testHandlers[ev.name].ready(ev);
    }
    xsrt.addEventListener("testsetup", testSetup);
    xsrt.addEventListener("testready", testReady);
    xsrt.registerTests(Object.keys(testHandlers));

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
                
                React.createElement(Xaml.TextBlock, {name: "label1", grid$row: "0"}, host.getState().text.length), 
                React.createElement(MultiLineTextBox, {
                    name: "textBox1", 
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

