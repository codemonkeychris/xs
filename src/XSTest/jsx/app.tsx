/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    var key = "fQVCnK180v01ogsuHZYq~a82hEegA-0yi1_n5Uo7kng~Aj1b4pyjrc02dm0OfXfRTtd0BkZsLdrtxMxBFX6Yqgsq3K3fdEelLTh-WtrIEgxI";

    export function setInitialState() {
        host.setState({ 
            center: ""
        });
    };

    function centerChanged(sender) {
        var loc = sender.center.position;

        host.setState({center: "alt:" + loc.altitude + ", lat:" + loc.latitude + ", lon:" + loc.longitude });
    }

    export function render() {

        return ( 
            <Xaml.Grid rows={["*", "auto"]}>
                <Xaml.MapControl mapServiceToken={key} onCenterChanged={centerChanged} />
                <Xaml.TextBlock grid$row={1} text={host.getState().center} />
            </Xaml.Grid>
        );
    }
}

