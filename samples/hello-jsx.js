var App;
(function (App) {

    App.setInitialState = function() {
        host.setState({x1: "Click Me!", frame : 0 });
    };
    
    function clicked() {
        host.setState({ x1: "Clicked!" });
    }

    setInterval(function() {
        var c = +host.getState().frame;
        host.setState({frame: c+1});
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
                        text={host.getState().frame + ':' + host.getState().x1} /> 
                    } />
            </Xaml.StackPanel>
        );
    }
    
})(App || (App = {}));
