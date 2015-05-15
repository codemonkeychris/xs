# Xaml Script (xs)

Xaml Script is an experiment in binding JavaScript to Xaml, using function/reactive 
programming techniques (insipred by ReactNative)

> Note: At this point contributions aren't supported

## Building

1. Install Windows 10 Insider Preview (10074 or later - //build/ version)
2. Install Visual Studio 2015 RC - https://www.visualstudio.com/products/visual-studio-2015-downloads-vs

## Running

`F5` from Visual Studio 2015 should work. 

The default program will be created in 
`C:\Users\$env:username\AppData\Local\Packages\XSTest_8z7hbd2ww68bp\RoamingState\xs-program.js`

When the program is running, you can edit the JavaScript file

## Basic design

The system works by hosting the JavaScript engine in a Xaml based Universal Windows App 
(UWA). When the file is changed, the app restarts, preserving the state of the app.

The structure of the program is:

```
var App;
(function (App) {

    App.setInitialState = function() {
        host.state.setState("x1", "Click Me!");
    };
    
    App.eventHandlers = {
        $1: function() {
            host.state.setState("x1", "Clicked!");
        }
    };
    
    App.render = function() {
        return { 
            type:'StackPanel', 
            name: 'root',
            children: [
                {
                    type:'TextBlock', 
                    name: 'header',
                    text:'Welcome to XS!', 
                    fontSize:56, 
                    margin:"30,10,10,10" 
                },
                {
                    type: 'Button',
                    name:'b1',
                    $click: '$2',
                    content: {
                        type:'TextBlock',
                        name:'t1',
                        text: 'O:' + host.state.getState("x1", "unset")
                    }  
                }
            ]
        };
    }
    
})(App || (App = {}));
```

At the moment, all the names are hard coded. Still TBD how this design lands, 
but this documents how it works today.

### App.setInitialState
In this function you should make calls to `host.state.setState` to populate the default
value for state. This will be reset only when the application is restarted.

### App.eventHandlers
This will eventually go away, for now, you need to have names that match your functions
from the `render` method. Typically you end up calling `host.state.setState` in event
handlers to update the app.

### App.render
This is the work horse of your program. You create a JSON block that defines the UI.
Different (for now) than React, this is a pure data block, no function are allowed, etc.
Each node has a `type` member which is an abreviated WinRT type name, and then the
set of members that we have implemented the parsing for.

The system is smart and diffs the JSON between calls to render and performs the minimal 
updates to the Xaml tree.

