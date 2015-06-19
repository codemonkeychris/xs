﻿/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            items: [1,2,3,4,5,6,7,8,9,10],
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
                host.setState({ items: [1,2,3,4,5,6,7,8,9,10] });
                host.setState({state: 0});
                break;
        }
    }

    function render() {

        return (
            <Xaml.StackPanel
                horizontalAlignment='Stretch'
                verticalAlignment='Stretch'>
                
                <Xaml.Button onClick={clicked}>Click Me</Xaml.Button>
                <Xaml.StackPanel 
                    orientation={host.getState().clientWidth > host.getState().clientHeight ? "Horizontal" : "Vertical" }  
                    childrenTransitions={[<Xaml.RepositionThemeTransition />]} 
                    >{
                    host.getState().items.map(function (i) { return <Xaml.TextBlock margin='5,5,5,5' text={i.toString()} />; })
                }</Xaml.StackPanel>
            </Xaml.StackPanel>
        );
    }
    App.render = render;
})(App || (App = {}));

 