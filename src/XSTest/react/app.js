/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";
    function setInitialState() {
        host.setState({
            centerPoint: { latitude: 47.5219, longitude: -122.5875 }
        });
    }
    App.setInitialState = setInitialState;
    ;
    function centerChanged(sender) {
        var loc = sender.center.position;
        host.setState({ centerPoint: { latitude: loc.latitude, longitude: loc.longitude } });
    }
    function renderPin() {
        return React.createElement(Xaml.Rectangle, {"map$location": { latitude: 47.55, longitude: -122.35 }, "fill": 'yellow', "width": 50, "height": 50});
    }
    function render() {
        var center = { latitude: 47.55, longitude: -122.35 };
        function sqaureCoords(centerPoint, width, height) {
            return [
                { latitude: centerPoint.latitude - (height / 2), longitude: centerPoint.longitude - (width / 2) },
                { latitude: centerPoint.latitude + (height / 2), longitude: centerPoint.longitude - (width / 2) },
                { latitude: centerPoint.latitude + (height / 2), longitude: centerPoint.longitude + (width / 2) },
                { latitude: centerPoint.latitude - (height / 2), longitude: centerPoint.longitude + (width / 2) }
            ];
        }
        var polygon = React.createElement(Xaml.MapPolygon, {"path": sqaureCoords(center, .1, .1)});
        return (React.createElement(Xaml.Grid, {"rows": ["*", "auto"]}, React.createElement(Xaml.MapControl, {"zoomLevel": 7.3, "center": host.getState().centerPoint, "mapServiceToken": key, "onCenterChanged": centerChanged, "mapElements": [polygon]}), React.createElement(Xaml.TextBlock, {"fontSize": 18, "grid$row": 1, "text": host.getState().center})));
    }
    App.render = render;
})(App || (App = {}));
