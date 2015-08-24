/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    export function setInitialState() {
        host.setState({ 
            sliderPos: 3,
            count: 1000
        });
    };

    function renderItem(item) {

        var idx = (item / host.getState().sliderPos) | 0;
        var stripped = idx % 2 == 0;

        return (
            <Xaml.Grid background={ stripped ? 'silver' : 'white' }>
                <Xaml.TextBlock 
                    text={"Item:" + item} />
            </Xaml.Grid>
        );
    }

    function range(min, max) {
        var res = [];
        for (var i=min; i<max; i++) {
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

    export function render() {
        return (
            <Xaml.Grid rows={['auto', 'auto', '*']}>
                <Xaml.Slider 
                    grid$row={0}
                    minimum={1} maximum={20} value={host.getState().sliderPos} 
                    onValueChanged={sliderChanged} />
                <Xaml.Slider 
                    grid$row={1}
                    minimum={1} maximum={2000} value={host.getState().count} 
                    onValueChanged={countChanged} />
                <Xaml.ListView 
                    itemContainerTransitions={null} 
                    grid$row={2} 
                    margin='10,10,10,10'
                    itemsSource={range(0,host.getState().count || 1000).map(renderItem)} />
            </Xaml.Grid>
        );
    }
}

