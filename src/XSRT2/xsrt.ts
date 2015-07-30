declare var host: any;
declare var xsrt: any;

module XsrtJS {
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
    xsrt.addEventListener('render', render);
    xsrt.addEventListener('command', command);
    if (App && App.setInitialState) {
        if (!xsrt.isInitialized) {
            xsrt.isInitialized = true;
            App.setInitialState();
        }
    }
}

module React {
    declare var App: any;

    function applyMembers(result, members) {
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
                else if (key == "style") {
                    applyMembers(result, value);
                }
                else {
                    result[key] = members[key];
                }
            }
        }
    }

    var funcCount = 1;
    export function createElement(ctor, members) {
        var result = new ctor();
        applyMembers(result, members);
        if (arguments.length > 2) {
            result.children = Array.prototype.slice.call(arguments, 2);
        }
        return result;
    }
}

module Xaml {
    export interface FrameworkElementProps {
        name?: string;
        width?: number;
        height?: number;
    }
    export interface ViewboxProps extends FrameworkElementProps {
        stretch?: string;
        stretchDirection?: string;
        child: any;
    }
    export class Viewbox { 
        type: string;
        constructor() {
            this.type = 'Viewbox';
        }
        props: ViewboxProps
    };
    export class ScrollViewer {
        type: string;
        constructor() {
            this.type = 'ScrollViewer';
        }
        props: FrameworkElementProps
    };
    export class GridView {
        type: string;
        constructor() {
            this.type = 'GridView';
        }
        props: FrameworkElementProps
    };
    export class ListView {
        type: string;
        constructor() {
            this.type = 'ListView';
        }
        props: FrameworkElementProps
    };
    export class Grid {
        type: string;
        constructor() {
            this.type = 'Grid';
        }
        props: FrameworkElementProps
    };
    export class StackPanel {
        type: string;
        constructor() {
            this.type = 'StackPanel';
        }
        props: FrameworkElementProps
    };
    export class Image {
        type: string;
        constructor() {
            this.type = 'Image';
        }
        props: FrameworkElementProps
    };
    export class TextBlock {
        type: string;
        constructor() {
            this.type = 'TextBlock';
        }
        props: FrameworkElementProps
    };
    export class Button {
        type: string;
        constructor() {
            this.type = 'Button';
        }
        props: FrameworkElementProps
    };
    export class CheckBox {
        type: string;
        constructor() {
            this.type = 'CheckBox';
        }
        props: FrameworkElementProps
    };
    export class Slider {
        type: string;
        constructor() {
            this.type = 'Slider';
        }
        props: FrameworkElementProps
    };
    export class ProgressBar {
        type: string;
        constructor() {
            this.type = 'ProgressBar';
        }
        props: FrameworkElementProps
    };
    export class RichEditBox {
        type: string;
        constructor() {
            this.type = 'RichEditBox';
        }
        props: FrameworkElementProps
    };
    export class TextBox {
        type: string;
        constructor() {
            this.type = 'TextBox';
        }
        props: FrameworkElementProps
    };
    export class ListBox {
        type: string;
        constructor() {
            this.type = 'ListBox';
        }
        props: FrameworkElementProps
    };
    export class ComboBox {
        type: string;
        constructor() {
            this.type = 'ComboBox';
        }
        props: FrameworkElementProps
    };
    export class CalendarDatePicker {
        type: string;
        constructor() {
            this.type = 'CalendarDatePicker';
        }
        props: FrameworkElementProps
    };
    export class CalendarView {
        type: string;
        constructor() {
            this.type = 'CalendarView';
        }
        props: FrameworkElementProps
    };
    export class RelativePanel {
        type: string;
        constructor() {
            this.type = 'RelativePanel';
        }
        props: FrameworkElementProps
    };
    export class RepositionThemeTransition {
        type: string;
        constructor() {
            this.type = 'RepositionThemeTransition';
        }
        props: FrameworkElementProps
    };
}
