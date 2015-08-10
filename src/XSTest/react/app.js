/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function renderItem(item) {
        return (React.createElement(Xaml.StackPanel, {"orientation": 'Horizontal'}, React.createElement(Xaml.TextBlock, {"text": item, "fontWeight": 'Bold'})));
    }
    function render() {
        return (React.createElement(Xaml.Grid, null, React.createElement(Xaml.ListView, {"margin": '10,10,10,10', "itemsSource": Object.keys(Xaml).map(renderItem)})));
    }
    App.render = render;
})(App || (App = {}));
