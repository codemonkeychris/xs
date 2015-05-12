function render(ev) {
    try {
        ev.view = JSON.stringify((App && App.render) ? App.render() : { type: 'TextBlock', text: 'Error: App.render not found' });
    }
    catch (e) {
        ev.view = JSON.stringify({ type: 'TextBlock', text: 'Error: ' + e });
    }
}
function command(ev) {
    var handler = App && App.eventHandlers && App.eventHandlers[ev.commandHandlerToken];
    if (handler) { handler(ev.sender, ev.eventArgs); }
}
host.state.addEventListener('render', render);
host.state.addEventListener('command', command);
if (App && App.setInitialState) { App.setInitialState(); }
