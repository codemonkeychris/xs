/// <reference path='../../xsrt2/xsrt.d.ts' />
var App;
(function (App) {
    function setInitialState() {
        var initialText = JSON.stringify({ type: 'TextBlock', text: "hello world" });
        host.setState({
            todos: [
                { value: 'Be awesome', done: false },
                { value: 'Learn React', done: true },
                { value: 'Use JSX in my CodePens', done: true }
            ],
            inputValue: ''
        });
    }
    App.setInitialState = setInitialState;
    ;
    function addTodo() {
        var todos = host.getState().todos;
        todos.push({ value: host.getState().inputValue, done: false });
        host.setState({ todos: todos, inputValue: '' });
    }
    function textChanged(sender, e) {
        host.setState({ inputValue: sender.text });
    }
    function addClicked(sender, e) {
        addTodo();
    }
    function markTodoDone(index) {
        var todos = host.getState().todos;
        var todo = host.getState().todos[index];
        todos.splice(index, 1);
        todo.done = !todo.done;
        todo.done ? todos.push(todo) : todos.unshift(todo);
        host.setState({
            todos: todos
        });
    }
    function renderTodo(todo, index) {
        return (React.createElement(Xaml.CheckBox, {"content": todo.value, "isChecked": todo.done, "onChecked": markTodoDone.bind(this, index)}));
    }
    function render() {
        return (React.createElement(Xaml.Grid, {"horizontalAlignment": 'Stretch', "verticalAlignment": 'Stretch', "rows": ['*', 'auto'], "columns": ['*', 'auto']}, React.createElement(Xaml.TextBox, {"grid$row": 1, "grid$column": 0, "placeholderText": 'What do you need to do?', "onTextChanged": textChanged, "text": host.getState().inputValue}), React.createElement(Xaml.Button, {"grid$row": 1, "grid$column": 1, "content": 'add', "onClick": addClicked}), React.createElement(Xaml.ListBox, {"grid$row": 0, "grid$column": 0, "grid$columnSpan": 2, "itemsSource": host.getState().todos.map(renderTodo)})));
    }
    App.render = render;
})(App || (App = {}));
