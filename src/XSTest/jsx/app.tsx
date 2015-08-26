/// <reference path='../../xsrt2/xsrt.d.ts' />
function circle(radius: number) { 
    return <Xaml.Ellipse width={radius} height={radius} fill="Black" />
}
function hstack(children) {
    return <Xaml.StackPanel orientation='Horizontal'>{children}</Xaml.StackPanel>
}
function vstack(children) {
    return <Xaml.StackPanel orientation='Vertical'>{children}</Xaml.StackPanel>
}
function range(min,max) {
    var result = []; 
    for (var i=min; i<max; i++) {
        result.push(i);
    }
    return result;
}

module App {

    interface Entry {
        inputText: string;
        result: any;
    }

    export function setInitialState() {
        var helloWorld = doEval("t = " + JSON.stringify({ type: 'TextBlock', text: "hello world" }));
        var sample = doEval("hstack(range(1,5).map(function(i) { return i * 10; }).map(circle))");
        host.setState({ 
            text: "",
            history: [
                helloWorld,
                { inputText: "1+1", result: 1+1 },
                sample
            ]
        });
    };

    function textChanged(sender, e) {
        host.setState({ text: sender.text });
    }
    function evalClicked(sender, e) {
        var entry = doEval(host.getState().text);

        var history = host.getState().history;
        history.push(entry);
        host.setState({history: history, text: ''});
    }
    function doEval(input:string) {
        var entry : Entry;
        var result : any;
        try {
            result = eval(input);
        }
        catch (e) {
            result = "" + e;
        }

        try {
            entry = { 
                inputText: input, 
                result: JSON.parse(JSON.stringify(result)) 
            };
        }
        catch (e) {
            entry = { inputText: input, result: result };
        }
        return entry;
    }

    function renderEntry(e : Entry) {
        return (
            <Xaml.StackPanel>
                <Xaml.TextBlock text={">" + e.inputText} />
                <Xaml.ContentControl content={e.result} />
            </Xaml.StackPanel>
        );
    }

    function EntryBox() {
        return <Xaml.TextBox
            fontFamily='Consolas'
            fontSize={14}
            width={450}
            horizontalAlignment='Stretch'
            verticalAlignment='Stretch' />
    }

    export function render() {

        return ( 
            <Xaml.ScrollViewer>
                <Xaml.StackPanel 
                    horizontalAlignment='Stretch'
                    verticalAlignment='Stretch' >
                
                    <Xaml.StackPanel>{host.getState().history.map(renderEntry)}</Xaml.StackPanel>

                    <Xaml.StackPanel orientation='Horizontal'>
                        <EntryBox
                            onTextChanged={textChanged}
                            text={host.getState().text}  />
                        <Xaml.Button
                            content='eval'
                            onClick={evalClicked}
                            />
                    </Xaml.StackPanel>

                </Xaml.StackPanel>
            </Xaml.ScrollViewer>
        );
    }
}

