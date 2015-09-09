/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";

    export function setInitialState() {
        host.setState({ 
            centerPoint: {latitude:47.5219, longitude:-122.5875 }
        });
    };

    function centerChanged(sender) {
        var loc = sender.center.position;
        host.setState({centerPoint: {latitude:loc.latitude, longitude:loc.longitude}});
    }

    function renderPin() {
        return <Xaml.Rectangle map$location={{latitude:47.55, longitude:-122.35 }} fill='yellow' width={50} height={50} />
    }

    export function render() {
        var center = {latitude:47.55, longitude:-122.35 };
        function sqaureCoords(centerPoint : Xaml.Geopoint, width, height) : Xaml.Geopoint[] {
            return [
                {latitude:centerPoint.latitude-(height/2), longitude:centerPoint.longitude-(width/2) },
                {latitude:centerPoint.latitude+(height/2), longitude:centerPoint.longitude-(width/2) },
                {latitude:centerPoint.latitude+(height/2), longitude:centerPoint.longitude+(width/2) },
                {latitude:centerPoint.latitude-(height/2), longitude:centerPoint.longitude+(width/2) }
            ];
        }

        var polygon = <Xaml.MapPolygon path={sqaureCoords(center, .1, .1)} />;

        return ( 
            <Xaml.Grid rows={["*", "auto"]}>
                <Xaml.MapControl 
                    zoomLevel={7.3} 
                    center={host.getState().centerPoint} 
                    mapServiceToken={key} 
                    onCenterChanged={centerChanged} 
                    mapElements={[ polygon ]}/>
                <Xaml.TextBlock fontSize={18} grid$row={1} text={host.getState().center} />
            </Xaml.Grid>
        );
    }
}

