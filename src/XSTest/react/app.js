/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            items: [1,2,3],
            state: 0
        });
    };

    function clicked(sender, e) {
        switch (host.getState().state) {
            case 0:
                host.setState({ items: [2,4] });
                host.setState({state: 1});
                break;
            case 1:
                host.setState({ items: [4,5,6,7] });
                host.setState({state: 2});
                break;
            case 2:
                host.setState({ items: [1,2] });
                host.setState({state: 3});
                break;
            case 3:
                host.setState({ items: [1,2,3] });
                host.setState({state: 0});
                break;
        }
    }

    function render() {

        return (
            React.createElement(Xaml.StackPanel, {
                horizontalAlignment: "Stretch", 
                verticalAlignment: "Stretch"}, 
                
                React.createElement(Xaml.Button, {onClick: clicked}, "Click Me"), 
                React.createElement(Xaml.StackPanel, null, 
                    host.getState().items.map(function (i) { return React.createElement(Xaml.TextBlock, {text: i.toString()}); })
                )
            )
        );
    }
    App.render = render;
})(App || (App = {}));

 