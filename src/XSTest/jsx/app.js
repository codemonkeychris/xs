/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.state.setState("x1", "Click Me!");
        host.state.setState("prefix", "Item");
        host.state.setState("sliderPos", 1);
        host.state.setState("filter", "true");
    };

    function sliderChanged(sender, e) {
        host.state.setState("sliderPos", e.newValue);
    }

    function buttonClicked() {
        host.state.setState("x1", "Clicked!");
    }

    function textChanged(sender, e) {
        host.state.setState("prefix", sender.text);
    }

    function checked(sender, e) {
        host.state.setState("filter", host.helpers.getIsChecked(sender));
    }

    function render() {
        return (
            <Xaml.Grid name='root' 
                rows={['auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto']}
                columns={['auto']} >
                <Xaml.TextBlock 
                    name='header' 
                    grid$row='0' 
                    text='Welcome to XamlScript' 
                    fontSize='36' margin='10,10,10,10' />
                <Xaml.Button name='b1' grid$row='1' 
                    onClick={buttonClicked}
                    content={<Xaml.TextBlock name='t1' text={'O:' + host.state.getState("x1", "unset")} />} />
                <Xaml.CheckBox name='c1' grid$row='2' content='Filter' 
                    onClick={checked}
                    isChecked={host.state.getState("filter", "true") == "true"} />
                <Xaml.Slider name='slider1'
                    grid$row='3'
                    minimum='1' maximum='20' value='5' 
                    onValueChanged={sliderChanged} />
                <Xaml.TextBox
                    name='text1'
                    grid$row='4'
                    fontFamily='Consolas'
                    fontSize='20'
                    onTextChanged={textChanged}
                    text={host.state.getState("prefix", "Item")}  />
                <Xaml.TextBox
                    name='text2'
                    grid$row='5'
                    fontFamily='Consolas'
                    fontSize='20'
                    text={'' + host.state.getState("sliderPos", "n/a")} />
                <Xaml.StackPanel
                    name='nest1'
                    grid$row='6'>{

                    [5,6,7,8,9,10,11,12,13,14,15,16,17].
                    filter(function (i) { return host.state.getState("filter", "true") != "true" || i % 2 == 1 }).
                    map(function (i) { 
                        return <Xaml.TextBlock
                            name={'item' + i}
                            text={host.state.getState("prefix", "Item")  + ':' + i} 
                            fontSize={(+host.state.getState("sliderPos", "0")  + i)*2} />
                    })
                }</Xaml.StackPanel>
            </Xaml.Grid>
        );
    }
    App.render = render;
})(App || (App = {}));
