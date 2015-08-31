/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";
    function setInitialState() {
        host.setState({
            center: "",
            zoomLevel: 7.3,
            centerPoint: { latitude: 47.5219, longitude: -122.5875 }
        });
    }
    App.setInitialState = setInitialState;
    ;
    function centerChanged(sender) {
        var loc = sender.center.position;
        host.setState({ centerPoint: { latitude: loc.latitude, longitude: loc.longitude } });
        host.setState({ center: "zoom:" + sender.zoomLevel + ", lat:" + loc.latitude + ", lon:" + loc.longitude });
    }
    function zoomChanged(sender) {
        host.setState({ zoomLevel: sender.zoomLevel });
    }
    function renderPin() {
        return React.createElement(Xaml.Button, {"map$location": { latitude: 47.5219, longitude: -122.5875 }, "content": 'home!!!'});
    }
    function render() {
        return (React.createElement(Xaml.Grid, {"rows": ["*", "auto"]}, React.createElement(Xaml.MapControl, {"zoomLevel": host.getState().zoomLevel, "center": host.getState().centerPoint, "mapServiceToken": key, "onCenterChanged": centerChanged}, React.createElement(Xaml.MapItemsControl, {"itemsSource": [renderPin()]})), React.createElement(Xaml.TextBlock, {"fontSize": 18, "grid$row": 1, "text": host.getState().center})));
    }
    App.render = render;
})(App || (App = {}));
