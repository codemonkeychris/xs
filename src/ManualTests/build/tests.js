/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            mode: "loading",
            text: "test"
        });
    };

    var testHandlers = {
        calendarview: {
            state: {
            },
            ready: function(ev) {
                var c1 = ev.root.findName("c1");
                xsrt.assert(c1, "c1 should exist");
                xsrt.assert(c1.minDate, "minDate should be set");
                xsrt.assert(c1.maxDate, "maxDate should be set");

                var c2 = ev.root.findName("c2");
                xsrt.assert(c2, "c2 should exist");
                xsrt.assert(c2.date, "date should be set: " + c2.date);
            },
            render: function() {
                return (
                    React.createElement(Xaml.StackPanel, {horizontalAlignment: "Stretch", verticalAlignment: "Stretch"}, 
                        React.createElement(Xaml.CalendarView, {name: "c1", minDate: "6/5/2015", maxDate: "6/7/2015"}), 
                        React.createElement(Xaml.CalendarDatePicker, {name: "c2", date: "6/5/2015"})
                    )
                );
            }
        },
        couple_controls: {
            state: {
                text: "a",
                n: 5
            },
            ready: function(ev) {
                var b1 = ev.root.findName("b1");
                xsrt.assert(b1, "b1 should exist");
                var p1 = ev.root.findName("p1");
                xsrt.assert(p1, "p1 should exist");
                xsrt.assert(xsrt.getRangeValue(p1) == 5, "p1.value should be set to 5 is '" + xsrt.getRangeValue(p1) + "'");
                var s1 = ev.root.findName("s1");
                xsrt.assert(s1, "s1 should exist");
                xsrt.assert(xsrt.getRangeValue(s1) == 5, "s1.value should be set to 5");
            },
            render: function() {
                return (
                    React.createElement(Xaml.StackPanel, {horizontalAlignment: "Stretch", verticalAlignment: "Stretch"}, 
                        React.createElement(Xaml.Button, {name: "b1", content: "hello"}), 
                        React.createElement(Xaml.ProgressBar, {name: "p1", minimum: "0", maximum: "10", value: host.getState().n}), 
                        React.createElement(Xaml.Slider, {name: "s1", minimum: "0", maximum: "10", value: host.getState().n})
                    )
                );
            }
        },
        fg_color: {
            state: {
                text: "a"
            },
            ready: function(ev) {
                var label1 = ev.root.findName("label1");
                xsrt.assert(label1, "label1 should exist");
                xsrt.assert(label1 && label1.text === "1", "should have number '1' in label");
                xsrt.assert(label1 && label1.foreground, "should have foreground label");
            },
            render: function() {
                return (
                    React.createElement(Xaml.Grid, {
                        horizontalAlignment: "Stretch", 
                        verticalAlignment: "Stretch", 
                        rows: ['auto', '*'], 
                        columns: ['*']}, 
                
                        React.createElement(Xaml.TextBlock, {foreground: "red", name: "label1", grid$row: "0"}, host.getState().text.length)
                    )
                );
            }
        },
        simple_test_1: {
            state: {
                text: "a"
            },
            ready: function(ev) {
                var label1 = ev.root.findName("label1");
                xsrt.assert(label1, "label1 should exist");
                xsrt.assert(label1 && label1.text === "1", "should have number '1' in label");
            },
            render: function() {
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
        },
        simple_test_2: {
            state: {
                text: "ab"
            },
            ready: function(ev) {
                var label2 = ev.root.findName("label2");
                xsrt.assert(label2, "label2 should exist");
                xsrt.assert(label2 && label2.text === "2", "should have number '2' in label");

                var textBox1 = ev.root.findName("textBox1");
                xsrt.assert(textBox1, "textBox1 should exist");
                xsrt.assert(textBox1 && textBox1.text === "ab", "should have number 'ab' in textBox1");
            },
            render: function() {
                return (
                    React.createElement(Xaml.Grid, {
                        horizontalAlignment: "Stretch", 
                        verticalAlignment: "Stretch", 
                        rows: ['auto', '*'], 
                        columns: ['*']}, 
                
                        React.createElement(Xaml.TextBlock, {name: "label2", grid$row: "0"}, host.getState().text.length), 
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
        }
    }
    function testSetup(ev) {
        var s = testHandlers[ev.name].state;
        s.mode = "running";
        s.activeTest = ev.name;
        host.setState(s);
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
            verticalAlignment: "Stretch"});
    }

    function renderResults() {
        var logs = xsrt.getLogs();
        var lastTest = "";
        var testIndex = -1;

        return (
            React.createElement(Xaml.ScrollViewer, null, 
                React.createElement(Xaml.StackPanel, null, 
                    Array.prototype.map.call(logs, function (entry, index) {
                        if (lastTest !== entry.test) { testIndex++; }
                        lastTest = entry.test;

                        return (
                            React.createElement(Xaml.StackPanel, {orientation: "Horizontal", background: testIndex % 2 == 0 ? "#EEE" : "#00FFFFFF"}, 
                                React.createElement(Xaml.TextBlock, {margin: "0,0,10,0", fontFamily: "Consolas", text:  entry.test}), 
                                React.createElement(Xaml.TextBlock, {fontFamily: "Consolas", text: (entry.result ? "       " : "failed:") + entry.message})
                            )
                        );    
                    })
                )
            )
        );
    }

    function render() {
        switch (host.getState().mode) {
            case "running":
                return testHandlers[host.getState().activeTest].render();
            case "results":
                return renderResults();
            default:
                return React.createElement(Xaml.TextBlock, {text: "loading..."})
        }
    }
    App.render = render;
})(App || (App = {}));

