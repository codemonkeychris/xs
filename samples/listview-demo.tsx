/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function renderItem(item) {
        return (
            <Xaml.Grid>
                <Xaml.TextBlock 
                    text={item} 
                    fontWeight={ item % 2 == 0 ? 'bold' : 'normal' } />
            </Xaml.Grid>
        );
    }

    function render() {
        return (
            <Xaml.Grid>
                <Xaml.ListView margin='10,10,10,10' itemsSource={[0,1,2,3,4,5].map(renderItem)} />
            </Xaml.Grid>
        );
    }
    App.render = render;
})(App || (App = {}));