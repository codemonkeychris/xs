/// <reference path='c:\repos\xs\src\xsrt2\xsrt.ts' />
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ 
            x1: "Click Me!",
            prefix: "Item",
            sliderPos: 1,
            filter: true,
            activePage: 'Demo1'
        });
    };

    var pages = ["Demo1", "Demo2"];

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
            React.createElement(Xaml.Grid, {
                rows: ['auto', 'auto', 'auto', 'auto', 'auto', 'auto', 'auto'], 
                columns: ['auto', 'auto', '*']}, 
                
                React.createElement(Xaml.Button, {
                    grid$row: "0", 
                    onClick: buttonClicked, 
                    content: React.createElement(Xaml.TextBlock, {name: "t1", text: 'O:' + host.getState().x1})}), 

                React.createElement(Xaml.CheckBox, {grid$row: "1", content: "Filter", 
                    onClick: checked, 
                    isChecked: host.getState().filter}), 

                React.createElement(Xaml.TextBlock, {name: "label1", grid$row: "2"}, "Scale factor"), 
                React.createElement(Xaml.Slider, {
                    grid$row: "2", 
                    grid$column: "1", 
                    acc$labeledBy: "label1", 
                    minimum: "1", maximum: "20", value: "5", 
                    onValueChanged: sliderChanged}), 
                React.createElement(Xaml.ProgressBar, {
                    grid$row: "3", 
                    grid$column: "1", 
                    minimum: "1", maximum: "20", value: host.getState().sliderPos}), 

                React.createElement(Xaml.TextBlock, {name: "label2", grid$row: "4"}, "Prefix"), 
                React.createElement(Xaml.TextBox, {
                    grid$row: "4", 
                    grid$column: "1", 
                    acc$labeledBy: "label2", 
                    fontFamily: "Consolas", 
                    fontSize: "20", 
                    onTextChanged: textChanged, 
                    text: host.getState().prefix}), 

                React.createElement(Xaml.TextBlock, {name: "label3", grid$row: "5"}, "ActivePage (output)"), 
                React.createElement(Xaml.TextBox, {
                    grid$row: "5", 
                    grid$column: "1", 
                    acc$labeledBy: "label3", 
                    fontFamily: "Consolas", 
                    fontSize: "20", 
                    text: '' + host.getState().activePage}), 
                React.createElement(Xaml.StackPanel, {
                    grid$columnSpan: "3", 
                    grid$row: "6"}, 

                    [5,6,7,8,9,10,11,12,13,14].
                    filter(function (i) { return !host.getState().filter || i % 2 == 1 }).
                    map(function (i) { 
                        return React.createElement(Xaml.TextBlock, {
                            text: host.getState().prefix  + ':' + i, 
                            fontSize: (host.getState().sliderPos  + i)*2})
                    })
                )
            )
        );
    }
    
    function renderDemo2() {
        return React.createElement(Xaml.TextBlock, {name: "page2", text: "UNDONE: page 2"})
    }

    function ActivePage() {
        switch (host.getState().activePage) {
            case "Demo1":
                return renderDemo1();
            case "Demo2":
                return renderDemo2();
            default:
                return React.createElement(Xaml.TextBlock, {name: "err1", text:  "Unknown page! State: " + JSON.stringify(host.getState())});
        }
    }

    function render() {

        return (
            React.createElement(Xaml.Grid, {
                rows: ['auto', 'auto', 'auto'], 
                columns: ['auto', 'auto', '*']}, 
                
                React.createElement(Xaml.TextBlock, {grid$row: "0", grid$columnSpan: "3", text: "Welcome to XS", fontSize: "36", margin: "10,10,10,10"}), 

                React.createElement(Xaml.ComboBox, {name: "scenarioSelector", 
                    grid$row: "1", 
                    itemsSource: pages, 
                    selectedItem: host.getState().activePage, 
                    onSelectionChanged: pageSelected, 
                    margin: "5,5,5,5"}), 

                React.createElement(ActivePage, {grid$row: "2"})
            )
        );
    }
    App.render = render;
})(App || (App = {}));
