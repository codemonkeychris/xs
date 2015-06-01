# Xaml Script (xs)

Xaml Script is an experiment in binding JavaScript to Xaml, using function/reactive 
programming techniques (insipred by ReactNative)

> Note: At this point contributions aren't supported

## Building

1. Install Windows 10 Insider Preview (10074 or later - //build/ version)
2. Install Visual Studio 2015 RC - https://www.visualstudio.com/products/visual-studio-2015-downloads-vs
3. Install JSX (react-tools) and TypeScript - required to `F5` in VS, as there is a custom build 
step that invokes JSX and TSC
```
npm install -g react-tools
npm install -g typescript
```

### Project structure

#### XSRT
This is a basic Chakra host, currently with as little policy as possible. The ValueMarshaller 
infrastructure is probably the most out there thing, but it needs to be integrated with the host.

#### XSRT2
This is the logic for the bulk of the XS project. Currently written in C# for expediency, but should 
be migrated to C++ to avoid the second GC in the process.

#### XSTest
Simple test host, ideally this should be replaced by a Yomen generator or something, this is really 
just file->new project.

## Running

`F5` from Visual Studio 2015 should work. 

The default program will be created in (PS syntax)
`C:\Users\$env:username\AppData\Local\Packages\XSTest_8z7hbd2ww68bp\RoamingState\xs-program.js`

When the program is running, you can edit the JavaScript file

## Using React

The recommended usage is to integrate using JSX syntax. The hacky React namespace provided handles
eventHandlers and creates the right object shape.

My recommended steps to edit using JSX is:

1. Edit `watch.ps1` to point to your user directory
4. Edit `app.js` from the root of XSTest, when you save the program will update

## Basic design

The system works by hosting the JavaScript engine in a Xaml based Universal Windows App 
(UWA). When the file is changed, the app restarts, preserving the state of the app.

The structure of the program is:

```
var App;
(function (App) {

    App.setInitialState = function() {
        host.setState({ x1: "Click Me!" });
    };
    
    App.eventHandlers = {
        $1: function() {
            host.setState({ x1: "Clicked!" });
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
                    onClick: '$1',
                    content: {
                        type:'TextBlock',
                        name:'t1',
                        text: 'O:' + host.getState().x1
                    }  
                }
            ]
        };
    }
    
})(App || (App = {}));
```

At the moment, all the names are hard coded. Still TBD how this design lands, 
but this documents how it works today.

This sample, converted to JSX is a bit nicer because the event handlers are
dealt with automatically.

```
var App;
(function (App) {
    App.setInitialState = function() {
        host.setState({ x1: "Click Me!" });
    };
    
    function clicked() {
        host.setState({ x1: "Clicked!" });
    }

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
                        text={'O:' + host.getState().x1 } /> 
                    } />
            </Xaml.StackPanel>
        );
    }
})(App || (App = {}));
```

### App.setInitialState
In this function you should make calls to `host.setState` to populate the default
value for state. This will be reset only when the application is restarted.

### App.eventHandlers
This will eventually go away, for now, you need to have names that match your functions
from the `render` method. Typically you end up calling `host.setState` in event
handlers to update the app.

### App.render
This is the work horse of your program. You create a JSON block that defines the UI.
Different (for now) than React, this is a pure data block, no function are allowed, etc.
Each node has a `type` member which is an abreviated WinRT type name, and then the
set of members that we have implemented the parsing for.

The system is smart and diffs the JSON between calls to render and performs the minimal 
updates to the Xaml tree.

### Host.setState & getState
This assigns new values to the global state object (using ES6 assign semantics) and
returns the global state object. The state is preserved across program changes, but
not currently serialized across process restarts.

## Controls
The set of controls, properties, and events available to the declaration of your
view are determined by the hand coded content in ClassHandlers.tt

> ClassHandlers.cs is an output file that is generated by VS' T4T compilation
> of ClassHandlers.tt

The set of available classes today are:
* TextBlock
* Image
* RichEditBox
* TextBox
* GridView
* ListView
* ComboBox
* ListBox
* Button
* CalendarDatePicker
* CalendarView
* RelativePanel
* RepositionThemeTransition
* ProgressBar
* Slider
* CheckBox
* StackPanel
* Grid

