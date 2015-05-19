var Xsrt;
(function (Xsrt) {
    function render(ev) {
        try {
            ev.view = JSON.stringify((App && App.render) ? App.render() : { type: 'TextBlock', text: 'Error: App.render not found' });
        }
        catch (e) {
            ev.view = JSON.stringify({ type: 'TextBlock', text: 'Error: ' + e });
        }
    }
    function command(ev) {
        try {
            var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
            if (handler) {
                handler(ev.sender, ev.eventArgs);
            }
        }
        catch (e) {
        }
    }
    host.state.addEventListener('render', render);
    host.state.addEventListener('command', command);
    if (App && App.setInitialState) {
        if (!host.state.isInitialized) {
            host.state.isInitialized = true;
            App.setInitialState();
        }
    }
})(Xsrt || (Xsrt = {}));
var React;
(function (React) {
    var funcCount = 1;
    function createElement(ctor, members) {
        var result = ctor();
        if (members) {
            var keys = Object.keys(members);
            for (var i = 0; i < keys.length; i++) {
                var key = keys[i];
                var value = members[key];
                if (value instanceof Function) {
                    var funName = "$" + (funcCount++);
                    App.eventHandlers = App.eventHandlers || {};
                    App.eventHandlers[funName] = value;
                    result[key] = funName;
                }
                else {
                    result[key] = members[key];
                }
            }
        }
        if (arguments.length > 2) {
            result.children = Array.prototype.slice.call(arguments, 2);
        }
        return result;
    }
    React.createElement = createElement;
})(React || (React = {}));
var Xaml;
(function (Xaml) {
    function Grid() { return { type: 'Grid' }; }
    Xaml.Grid = Grid;
    ;
    function StackPanel() { return { type: 'StackPanel' }; }
    Xaml.StackPanel = StackPanel;
    ;
    function TextBlock() { return { type: 'TextBlock' }; }
    Xaml.TextBlock = TextBlock;
    ;
    function Button() { return { type: 'Button' }; }
    Xaml.Button = Button;
    ;
    function CheckBox() { return { type: 'CheckBox' }; }
    Xaml.CheckBox = CheckBox;
    ;
    function Slider() { return { type: 'Slider' }; }
    Xaml.Slider = Slider;
    ;
    function TextBox() { return { type: 'TextBox' }; }
    Xaml.TextBox = TextBox;
    ;
})(Xaml || (Xaml = {}));
