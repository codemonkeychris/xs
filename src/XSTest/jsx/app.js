/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            x1: "Click Me!",
            prefix: "Item",
            sliderPos: 1,
            filter: true
        });
    };

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

    function render() {
        return (
            <Xaml.Grid 
                rows={['auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto']}
                columns={['auto', 'auto', '*']} >
                
                <Xaml.TextBlock grid$row='0' grid$columnSpan='3' text='Welcome to XS' fontSize='36' margin='10,10,10,10' />

                <Xaml.Button grid$row='1' 
                    onClick={buttonClicked}
                    content={<Xaml.TextBlock name='t1' text={'O:' + host.getState().x1} />} />

                <Xaml.CheckBox grid$row='2' content='Filter' 
                    onClick={checked}
                    isChecked={host.getState().filter} />

                <Xaml.TextBlock name='label1' grid$row='3'>Scale factor</Xaml.TextBlock>
                <Xaml.Slider 
                    grid$row='3'
                    grid$column='1'
                    acc$labeledBy='label1'
                    minimum='1' maximum='20' value='5' 
                    onValueChanged={sliderChanged} />
                <Xaml.ProgressBar 
                    grid$row='4'
                    grid$column='1'
                    minimum='1' maximum='20' value={host.getState().sliderPos} />

                <Xaml.TextBlock name='label2' grid$row='5'>Prefix</Xaml.TextBlock>
                <Xaml.TextBox
                    grid$row='5'
                    grid$column='1'
                    acc$labeledBy='label2'
                    fontFamily='Consolas'
                    fontSize='20'
                    onTextChanged={textChanged}
                    text={host.getState().prefix}  />

                <Xaml.TextBlock name='label3' grid$row='6'>Scale (output)</Xaml.TextBlock>
                <Xaml.TextBox
                    grid$row='6'
                    grid$column='1'
                    acc$labeledBy='label3'
                    fontFamily='Consolas'
                    fontSize='20'
                    text={'' + host.getState().sliderPos} />
                <Xaml.StackPanel
                    grid$columnSpan='3'
                    grid$row='7'>{

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
    App.render = render;
})(App || (App = {}));
