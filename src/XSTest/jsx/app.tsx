/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    export function setInitialState() {
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

    export function render() {

        return (
            <Xaml.Grid 
                horizontalAlignment='Stretch'
                verticalAlignment='Stretch'
                rows={['auto', '*', 'auto']}
                columns={['*']} >
                
                <Xaml.TextBlock name='label1' grid$row={0}>{"count:" + host.getState().text.length }</Xaml.TextBlock>
                <Xaml.TextBlock name='label2' grid$row={2}>Type in the text box</Xaml.TextBlock>
                <MultiLineTextBox
                    grid$row={1}
                    grid$column={0}
                    fontFamily='Consolas'
                    fontSize={18}
                    onTextChanged={textChanged}
                    text={host.getState().text}  />
            </Xaml.Grid>
        );
    }
}