/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";
    function setInitialState() {
        host.setState({
            center: ""
        });
    }
    App.setInitialState = setInitialState;
    ;
    function centerChanged(sender) {
        host.setState({ center: sender.center.toString() });
    }
    function render() {
        return (React.createElement(Xaml.Grid, {"rows": ["*", "auto"]}, React.createElement(Xaml.MapControl, {"mapServiceToken": key, "onCenterChanged": centerChanged}), React.createElement(Xaml.TextBlock, {"text": host.getState().center})));
    }
    App.render = render;
})(App || (App = {}));
