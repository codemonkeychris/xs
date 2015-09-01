/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";

    export function setInitialState() {
        host.setState({ 
            center: "",
            zoomLevel: 7.3,
            centerPoint: {latitude:47.5219, longitude:-122.5875 }
        });
    };

    function centerChanged(sender) {
        var loc = sender.center.position;

        host.setState({centerPoint: {latitude:loc.latitude, longitude:loc.longitude}});

        host.setState({center: "zoom:" + sender.zoomLevel + ", lat:" + loc.latitude + ", lon:" + loc.longitude });
    }
    function zoomChanged(sender) {
        host.setState({zoomLevel: sender.zoomLevel});
    }

    function renderPin() {
        return <Xaml.Rectangle map$location={{latitude:47.5219, longitude:-122.5875 }} fill='red' width={50} height={50} />
    }

    export function render() {

        return ( 
            <Xaml.Grid rows={["*", "auto"]}>
                <Xaml.MapControl 
                    zoomLevel={host.getState().zoomLevel} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged}>
                    <Xaml.MapItemsControl items={[renderPin()]} />
                </Xaml.MapControl>
                <Xaml.TextBlock fontSize={18} grid$row={1} text={host.getState().center} />
            </Xaml.Grid>
        );
    }
}

