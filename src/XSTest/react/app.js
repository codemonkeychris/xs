/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ date: '6/5/2015' });
    }
    function datePicked(sender, e) {
        host.setState({ date:sender.date.toDateString() });
    }

    function render() {
        return (
            React.createElement(Xaml.StackPanel, {horizontalAlignment: "Stretch", verticalAlignment: "Stretch"}, 
                React.createElement(Xaml.CalendarView, {name: "cal1", minDate: "6/5/2015 12:00am"}), 
                React.createElement(Xaml.CalendarDatePicker, {name: "cal2", date: host.getState().date, onDateChanged: datePicked})
            )
        );
    }
    App.render = render;
})(App || (App = {}));
