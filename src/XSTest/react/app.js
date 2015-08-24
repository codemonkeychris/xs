/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function setInitialState() {
        host.setState({
            sliderPos: 3,
            count: 1000
        });
    }
    App.setInitialState = setInitialState;
    ;
    function renderItem(item) {
        var idx = (item / host.getState().sliderPos) || 0;
        var stripped = idx % 2 == 0;
        return (React.createElement(Xaml.Grid, {"background": stripped ? 'silver' : 'white'}, React.createElement(Xaml.TextBlock, {"text": "Item:" + item})));
    }
    function range(min, max) {
        var res = [];
        for (var i = min; i < max; i++) {
            res.push(i);
        }
        return res;
    }
    function sliderChanged(sender, e) {
        host.setState({ sliderPos: e.newValue });
    }
    function countChanged(sender, e) {
        host.setState({ count: e.newValue });
    }
    function render() {
        return (React.createElement(Xaml.Grid, {"rows": ['auto', 'auto', '*']}, React.createElement(Xaml.Slider, {"grid$row": 0, "minimum": 1, "maximum": 20, "value": host.getState().sliderPos, "onValueChanged": sliderChanged}), React.createElement(Xaml.Slider, {"grid$row": 1, "minimum": 1, "maximum": 2000, "value": host.getState().count, "onValueChanged": countChanged}), React.createElement(Xaml.ListView, {"itemContainerTransitions": null, "grid$row": 2, "margin": '10,10,10,10', "itemsSource": range(0, host.getState().count || 1000).map(renderItem)})));
    }
    App.render = render;
})(App || (App = {}));
