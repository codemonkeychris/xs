/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            x1: "Click Me!",
            prefix: "Item",
            sliderPos: 1,
            filter: true,
            activePage: 'Demo1',
            frameCount: 0
        });
    };

    var pages = ["Demo1", "Demo2"];

    setInterval(function() {
        host.setState({ frameCount: host.getState().frameCount + 1 });
    }, 200)

    function sliderChanged(sender, e) {
        host.setState({ sliderPos: e.newValue });
    }

    function buttonClicked() {
        host.setState({ x1: "Clicked!" });
    }

    function textChanged(sender, e) {
        host.setState({ prefix: sender.text });
    }

    function checked(sender, e) {
        host.setState({ filter: host.helpers.getIsChecked(sender) });
    }

    function pageSelected(sender, e) {
        host.setState({ activePage: e.addedItems[0] });
    }

    function renderDemo1() {
        return (
            <Xaml.Grid 
                rows={['auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto']}
                columns={['auto', 'auto', '*']} >
                
                <Xaml.Button 
                    grid$row='0' 
                    onClick={buttonClicked}
                    content={<Xaml.TextBlock name='t1' text={'O:' + host.getState().x1} />} />

                <Xaml.CheckBox grid$row='1' content='Filter' 
                    onClick={checked}
                    isChecked={host.getState().filter} />

                <Xaml.TextBlock name='label1' grid$row='2'>Scale factor</Xaml.TextBlock>
                <Xaml.Slider 
                    grid$row='2'
                    grid$column='1'
                    acc$labeledBy='label1'
                    minimum='1' maximum='20' value={host.getState().sliderPos} 
                    onValueChanged={sliderChanged} />
                <Xaml.ProgressBar 
                    grid$row='3'
                    grid$column='1'
                    minimum='1' maximum='20' value={host.getState().sliderPos} />

                <Xaml.TextBlock name='label2' grid$row='4'>Prefix</Xaml.TextBlock>
                <Xaml.TextBox
                    grid$row='4'
                    grid$column='1'
                    acc$labeledBy='label2'
                    fontFamily='Consolas'
                    fontSize='20'
                    onTextChanged={textChanged}
                    text={host.getState().prefix}  />

                <Xaml.TextBlock name='label3' grid$row='5'>ActivePage (output)</Xaml.TextBlock>
                <Xaml.TextBox
                    grid$row='5'
                    grid$column='1'
                    acc$labeledBy='label3'
                    fontFamily='Consolas'
                    fontSize='20'
                    text={'' + host.getState().activePage} />
                <Xaml.StackPanel
                    grid$columnSpan='3'
                    grid$row='6'>{

                    [5,6,7,8,9,10,11,12,13,14].
                    filter(function (i) { return !host.getState().filter || i % 2 == 1 }).
                    map(function (i) { 
                        return <Xaml.TextBlock
                            text={host.getState().prefix  + ':' + i} 
                            fontSize={(host.getState().sliderPos  + i)*2} />
                    })
                }</Xaml.StackPanel>
            </Xaml.Grid>
        );
    }
    
    function renderDemo2() {
        function i1() { return "i1 => (frame: " + host.getState().frameCount + ")"; }

        switch (host.getState().frameCount % 5) {
            case 0:
                return (
                    <Xaml.StackPanel>
                        <Xaml.TextBlock name='i1' text={i1()} />
                        <Xaml.TextBlock name='i2' text="i2" />
                        <Xaml.TextBlock name='i3' text="i3" />
                        <Xaml.StackPanel>
                            <Xaml.TextBlock text="annon1" />
                            <Xaml.TextBlock text="annon2" />
                            <Xaml.TextBlock text="annon3" />
                        </Xaml.StackPanel>
                    </Xaml.StackPanel>
                );
            case 1:
                return (
                    <Xaml.StackPanel>
                        <Xaml.TextBlock name='i1' text={i1()} />
                        <Xaml.TextBlock name='i2' text="i2" />
                        <Xaml.TextBlock name='i3' text="i3" />
                        <Xaml.TextBlock text="annon1" />
                        <Xaml.TextBlock text="annon2" />
                        <Xaml.TextBlock text="annon3" />
                        <Xaml.StackPanel />
                    </Xaml.StackPanel>
                );
            case 2:
                return (
                    <Xaml.StackPanel>
                        <Xaml.TextBlock name='i1' text={i1()} />
                        <Xaml.TextBlock name='i2' text="i2" />
                        <Xaml.TextBlock text="annon1" />
                        <Xaml.TextBlock name='i3' text="i3" />
                        <Xaml.TextBlock text="annon2" />
                        <Xaml.StackPanel>
                            <Xaml.TextBlock text="annon3" />
                        </Xaml.StackPanel>
                    </Xaml.StackPanel>
                );
            case 3:
                return (
                    <Xaml.StackPanel>
                        <Xaml.TextBlock name='i1' text={i1()} />
                        <Xaml.TextBlock name='i2' text="i2" />
                        <Xaml.TextBlock text="annon1" />
                        <Xaml.TextBlock text="annon2" />
                        <Xaml.StackPanel>
                            <Xaml.TextBlock name='i3' text="i3" />
                            <Xaml.TextBlock text="annon3" />
                        </Xaml.StackPanel>
                    </Xaml.StackPanel>
                );
            case 4:
                return (
                    <Xaml.StackPanel>
                        <Xaml.TextBlock name='i1' text={i1()} />
                        <Xaml.TextBlock name='i2' text="i2" />
                        <Xaml.TextBlock text="annon1" />
                        <Xaml.TextBlock text="annon2" />
                        <Xaml.TextBlock text="annon3" />
                        <Xaml.StackPanel>
                            <Xaml.TextBlock name='i3' text="i3" />
                        </Xaml.StackPanel>
                    </Xaml.StackPanel>
                );
        }
    }

    function ActivePage() {
        switch (host.getState().activePage) {
            case "Demo1":
                return renderDemo1();
            case "Demo2":
                return renderDemo2();
            default:
                return <Xaml.TextBlock name='err1' text={ "Unknown page! State: " + JSON.stringify(host.getState()) } />;
        }
    }

    function render() {

        return (
            <Xaml.Grid 
                rows={['auto', 'auto', 'auto' ]}
                columns={['auto', 'auto', '*']} >
                
                <Xaml.TextBlock grid$row='0' grid$columnSpan='3' text='Welcome to XS' fontSize='36' margin='10,10,10,10' />

                <Xaml.ComboBox name='scenarioSelector' 
                    grid$row='1' 
                    itemsSource={pages} 
                    selectedItem={host.getState().activePage} 
                    onSelectionChanged={pageSelected}
                    margin='5,5,5,5' />

                <ActivePage grid$row='2' />
            </Xaml.Grid>
        );
    }
    App.render = render;
})(App || (App = {}));
