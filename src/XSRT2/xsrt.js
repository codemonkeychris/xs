var XsrtJS;
(function (XsrtJS) {
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
            if (handler) {
                handler(ev.sender, ev.eventArgs);
            }
        }
        catch (e) {
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
})(XsrtJS || (XsrtJS = {}));
var React;
(function (React) {
    function applyMembers(result, members) {
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
    function createElement(ctor, members) {
        var result = new ctor();
        applyMembers(result, members);
        if (arguments.length > 2) {
            result.children = Array.prototype.slice.call(arguments, 2);
        }
        return result;
    }
    React.createElement = createElement;
})(React || (React = {}));
var Xaml;
(function (Xaml) {
    var Viewbox = (function () {
        function Viewbox() {
            this.type = 'Viewbox';
        }
        return Viewbox;
    })();
    Xaml.Viewbox = Viewbox;
    ;
    var ScrollViewer = (function () {
        function ScrollViewer() {
            this.type = 'ScrollViewer';
        }
        return ScrollViewer;
    })();
    Xaml.ScrollViewer = ScrollViewer;
    ;
    var GridView = (function () {
        function GridView() {
            this.type = 'GridView';
        }
        return GridView;
    })();
    Xaml.GridView = GridView;
    ;
    var ListView = (function () {
        function ListView() {
            this.type = 'ListView';
        }
        return ListView;
    })();
    Xaml.ListView = ListView;
    ;
    var Grid = (function () {
        function Grid() {
            this.type = 'Grid';
        }
        return Grid;
    })();
    Xaml.Grid = Grid;
    ;
    var StackPanel = (function () {
        function StackPanel() {
            this.type = 'StackPanel';
        }
        return StackPanel;
    })();
    Xaml.StackPanel = StackPanel;
    ;
    var Image = (function () {
        function Image() {
            this.type = 'Image';
        }
        return Image;
    })();
    Xaml.Image = Image;
    ;
    var TextBlock = (function () {
        function TextBlock() {
            this.type = 'TextBlock';
        }
        return TextBlock;
    })();
    Xaml.TextBlock = TextBlock;
    ;
    var Button = (function () {
        function Button() {
            this.type = 'Button';
        }
        return Button;
    })();
    Xaml.Button = Button;
    ;
    var CheckBox = (function () {
        function CheckBox() {
            this.type = 'CheckBox';
        }
        return CheckBox;
    })();
    Xaml.CheckBox = CheckBox;
    ;
    var Slider = (function () {
        function Slider() {
            this.type = 'Slider';
        }
        return Slider;
    })();
    Xaml.Slider = Slider;
    ;
    var ProgressBar = (function () {
        function ProgressBar() {
            this.type = 'ProgressBar';
        }
        return ProgressBar;
    })();
    Xaml.ProgressBar = ProgressBar;
    ;
    var RichEditBox = (function () {
        function RichEditBox() {
            this.type = 'RichEditBox';
        }
        return RichEditBox;
    })();
    Xaml.RichEditBox = RichEditBox;
    ;
    var TextBox = (function () {
        function TextBox() {
            this.type = 'TextBox';
        }
        return TextBox;
    })();
    Xaml.TextBox = TextBox;
    ;
    var ListBox = (function () {
        function ListBox() {
            this.type = 'ListBox';
        }
        return ListBox;
    })();
    Xaml.ListBox = ListBox;
    ;
    var ComboBox = (function () {
        function ComboBox() {
            this.type = 'ComboBox';
        }
        return ComboBox;
    })();
    Xaml.ComboBox = ComboBox;
    ;
    var CalendarDatePicker = (function () {
        function CalendarDatePicker() {
            this.type = 'CalendarDatePicker';
        }
        return CalendarDatePicker;
    })();
    Xaml.CalendarDatePicker = CalendarDatePicker;
    ;
    var CalendarView = (function () {
        function CalendarView() {
            this.type = 'CalendarView';
        }
        return CalendarView;
    })();
    Xaml.CalendarView = CalendarView;
    ;
    var RelativePanel = (function () {
        function RelativePanel() {
            this.type = 'RelativePanel';
        }
        return RelativePanel;
    })();
    Xaml.RelativePanel = RelativePanel;
    ;
    var RepositionThemeTransition = (function () {
        function RepositionThemeTransition() {
            this.type = 'RepositionThemeTransition';
        }
        return RepositionThemeTransition;
    })();
    Xaml.RepositionThemeTransition = RepositionThemeTransition;
    ;
})(Xaml || (Xaml = {}));
