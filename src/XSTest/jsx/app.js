/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    function MyButton() {
        return <Xaml.Button margin='5,5,5,5' />;
    }

    function render() {
        return (
            <Xaml.StackPanel horizontalAlignment='Stretch' verticalAlignment='Stretch'>
                <Xaml.CalendarView name='cal1' minDate='6/5/2015 12:00am' />
                <Xaml.CalendarDatePicker />
            </Xaml.StackPanel>
        );
    }
    App.render = render;
})(App || (App = {}));
