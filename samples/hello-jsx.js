var App;
(function (App) {

    App.setInitialState = function() {
        host.state.setState("x1", "Click Me!");
        host.state.setState("frame", "0");
    };
    
    function clicked() {
        host.state.setState("x1", "Clicked!");
    }

    setInterval(function() {
        var c = +host.state.getState("frame", "0");
        host.state.setState("frame", c+1);
    }, 250);

    App.render = function() {
        return (
            <Xaml.StackPanel name='root'>
                <Xaml.TextBlock name='header' 
                    text='Welcome to XS!' 
                    fontSize='56'
                    margin='30,10,10,10' />
                <Xaml.Button name='b1' onClick={clicked}
                    content={<Xaml.TextBlock
                        name='t1'
                        text={host.state.getState('frame','0') + ':' + host.state.getState("x1", "unset")} /> 
                    } />
            </Xaml.StackPanel>
        );
    }
    
})(App || (App = {}));
