/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    function renderItem(item) {
        return (
            <Xaml.Grid>
                <Xaml.TextBlock 
                    text={item} 
                    fontWeight={ item % 2 == 0 ? 'bold' : 'normal' } />
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

    function items() {
        return range(0,10000).map(renderItem);
    }

    export function render() {
        return (
            <Xaml.Grid>
                <Xaml.ListView margin='10,10,10,10' itemsSource={items()} />
            </Xaml.Grid>
        );
    }
}

