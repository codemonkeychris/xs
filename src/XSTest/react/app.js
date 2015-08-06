/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function renderItem(item) {
        var x = 5;
        return (React.createElement(Xaml.Grid, null, React.createElement(Xaml.TextBlock, {"text": item, "fontWeight": item % 2 == 0 ? 'bold' : 'normal'})));
    }
    function render() {
        return (React.createElement(Xaml.Grid, null, React.createElement(Xaml.ListView, {"margin": '10,10,10,10', "itemsSource": [0, 1, 2, 3, 4, 5, 6, 7, 8].map(renderItem)})));
    }
    App.render = render;
})(App || (App = {}));
