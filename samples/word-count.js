/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            text: "test"
        });
    };

    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }

    function MultiLineTextBox() {
        return <Xaml.TextBox
            scrollViewer$horizontalScrollBarVisibility='Auto'
            scrollViewer$verticalScrollBarVisibility='Auto'
            acceptsReturn={true}
            textWrapping='Wrap'
            horizontalAlignment='Stretch'
            verticalAlignment='Stretch' />
    }

    function render() {

        return (
            <Xaml.Grid 
                horizontalAlignment='Stretch'
                verticalAlignment='Stretch'
                rows={['auto', '*']}
                columns={['*']} >
                
                <Xaml.TextBlock name='label1' grid$row='0'>{"count:" + host.getState().text.length }</Xaml.TextBlock>
                <MultiLineTextBox
                    grid$row='1'
                    grid$column='0'
                    fontFamily='Consolas'
                    fontSize='14'
                    onTextChanged={textChanged}
                    text={host.getState().text}  />
            </Xaml.Grid>
        );
    }
    App.render = render;
})(App || (App = {}));

