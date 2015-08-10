/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function renderItem(item) {
        return (
            <Xaml.StackPanel orientation='Horizontal'>
                <Xaml.TextBlock text={item} fontWeight='Bold'  />
                {
                    //Object.keys(Xaml[item].props).map(prop=><Xaml.TextBlock text={prop}  />)
                }
            </Xaml.StackPanel>
        );
    }

    function render() {
        return (
            <Xaml.Grid>
                <Xaml.ListView margin='10,10,10,10' itemsSource={Object.keys(Xaml).map(renderItem)} />
            </Xaml.Grid>
        );
    }
    App.render = render;
})(App || (App = {}));
