declare var host: any;

module Xsrt {
    // UNDONE: a bit of a hack... I want the "App" to see what we define here, but we 
    // also call into random things defined on "App"... noodle on this more
    //
    declare var App: any;

    function render(ev) {
        try {
            ev.view = JSON.stringify((App && App.render) ? App.render() : { type: 'TextBlock', text: 'Error: App.render not found' });
        }
        catch (e) {
            ev.view = JSON.stringify({ type: 'TextBlock', text: 'Error: ' + e });
        }
    }
    function command(ev) {
        try {
            var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
            if (handler) { handler(ev.sender, ev.eventArgs); }
        }
        catch (e) {
            // UNDONE: add exception pipe
        }
    }
    host.state.addEventListener('render', render);
    host.state.addEventListener('command', command);
    if (App && App.setInitialState) {
        if (!host.state.isInitialized) {
            host.state.isInitialized = true;
            App.setInitialState();
        }
    }
}