/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.state.setState("x1", "Click Me!");
        host.state.setState("prefix", "Item");
        host.state.setState("sliderPos", 1);
        host.state.setState("filter", true);
    };

    function sliderChanged(sender, e) {
        host.state.setState("sliderPos", e.newValue);
    }

    function buttonClicked() {
        host.state.setState("x1", "Clicked!");
    }

    function textChanged(sender, e) {
        host.state.setState("prefix", sender.text);
    }

    function checked(sender, e) {
        host.state.setState("filter", host.helpers.getIsChecked(sender));
    }

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
