/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return React.createElement(Xaml.Button, {margin: "5,5,5,5"});
    }

    function render() {
        return (
            React.createElement(Xaml.StackPanel, {horizontalAlignment: "Stretch", verticalAlignment: "Stretch"}, 
                React.createElement(Xaml.CalendarView, {name: "cal1", minDate: "6/5/2015 12:00am"})
            )
        );
    }
    App.render = render;
})(App || (App = {}));
