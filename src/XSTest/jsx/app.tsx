/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    export function setInitialState() {
        var initialText = JSON.stringify({ type: 'TextBlock', text: "hello world" });
        host.setState({ 
            text: initialText,
            content: JSON.parse(initialText)
        });
    };

    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }
    function refreshClicked(sender, e) {
        try {
            host.setState({ content: JSON.parse(host.getState().text) });
        }
        catch (e) {
            host.setState({ content: host.getState().text });
        }
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
                columns={['*', 'auto', '*']} >
                
                <MultiLineTextBox
                    grid$row={1}
                    grid$column={0}
                    fontFamily='Consolas'
                    fontSize={16}
                    onTextChanged={textChanged}
                    text={host.getState().text}  />

                <Xaml.Button
                    grid$row={1}
                    grid$column={1}
                    content='refresh'
                    onClick={refreshClicked}
                    />

                <Xaml.ContentControl 
                    grid$row={1}
                    grid$column={2}
                    content={host.getState().content}
                    />
            </Xaml.Grid>
        );
    }
}