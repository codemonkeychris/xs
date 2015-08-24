/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function renderItem(item) {
        return (React.createElement(Xaml.Grid, null, React.createElement(Xaml.TextBlock, {"text": item, "fontWeight": item % 2 == 0 ? 'bold' : 'normal'})));
    }
    function range(min, max) {
        var res = [];
        for (var i = min; i < max; i++) {
            res.push(i);
        }
        return res;
    }
    function items() {
        return range(0, 10000).map(renderItem);
    }
    function render() {
        return (React.createElement(Xaml.Grid, null, React.createElement(Xaml.ListView, {"margin": '10,10,10,10', "itemsSource": items()})));
    }
    App.render = render;
})(App || (App = {}));
