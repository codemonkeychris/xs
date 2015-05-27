declare var host: any;

module Xsrt {
    // UNDONE: a bit of a hack... I want the "App" to see what we define here, but we 
    // also call into random things defined on "App"... noodle on this more
    //
    declare var App: any;

    function render(ev) {
        try {
            ev.view = JSON.stringify((App && App.render) ? App.render() : { type: 'TextBlock', text: 'Error: App.render not found!' });
        }
        catch (e) {
            ev.view = JSON.stringify({ type: 'TextBlock', text: 'Error: ' + e });
        }
    }
    function command(ev) {
        try {
            var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
            if (handler) { handler(ev.sender, ev.eventArgs); }
        }
        catch (e) {
            // UNDONE: add exception pipe
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
}

module React {
    declare var App: any;

    var funcCount = 1;
    export function createElement(ctor, members) {
        var result = ctor();
        if (members) {
            var keys = Object.keys(members)
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
}

module Xaml {
    export function GridView() { return { type: 'GridView' } };
    export function ListView() { return { type: 'ListView' } };
    export function Grid() { return { type: 'Grid' } };
    export function StackPanel() { return { type: 'StackPanel' } };
    export function Image() { return { type: 'Image' } };
    export function TextBlock() { return { type: 'TextBlock' } };
    export function Button() { return { type: 'Button' } };
    export function CheckBox() { return { type: 'CheckBox' } };
    export function Slider() { return { type: 'Slider' } };
    export function ProgressBar() { return { type: 'ProgressBar' } };
    export function TextBox() { return { type: 'TextBox' } };
    export function ListBox() { return { type: 'ListBox' } };
    export function ComboBox() { return { type: 'ComboBox' } };
    export function CalendarDatePicker() { return { type: 'CalendarDatePicker' } };
    export function CalendarView() { return { type: 'CalendarView' } };
    export function RelativePanel() { return { type: 'RelativePanel' } };
    export function RepositionThemeTransition() { return { type: 'RepositionThemeTransition' } };
}
