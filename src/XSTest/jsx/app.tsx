/// <reference path='../../xsrt2/xsrt.d.ts' />
module App {
    
    // cloning from http://codepen.io/mfunkie/pen/BnzKx/?editors=001
    //

    // UNDONE: need to model React.createClass and props to correctly model 
    // the {todos} from the sample
    //

    export function setInitialState() {
        host.setState({ 
            todos: [
                { value:'Be awesome', done: false },
                { value:'Learn React', done: true },
                { value:'Use JSX in my CodePens', done: true }            
            ],
            inputValue: ''
        });
    };

    function addTodo() {
        var todos = host.getState().todos;
        todos.push({value:host.getState().inputValue, done: false });
        host.setState({ todos: todos, inputValue: ''});
    }

    function textChanged(sender, e) {
        host.setState({ inputValue: sender.text });
    }

    function markTodoDone(index : number) {
        var todos = host.getState().todos;
        var todo = host.getState().todos[index];
        todos.splice(index, 1);
        todo.done = !todo.done;

        todo.done ? todos.push(todo) : todos.unshift(todo);

        host.setState({
          todos: todos
        });
    }

    function renderTodo(todo : {value:string; done:boolean}, index : number) {
        return (
            <Xaml.CheckBox 
                content={todo.value} 
                isChecked={todo.done} 
                onChecked={markTodoDone.bind(this, index)} />
        );
    }

    export function render() {
      
        return ( 
            <Xaml.Grid 
                horizontalAlignment='Stretch'
                verticalAlignment='Stretch'
                rows={['auto', '*', 'auto']}
                columns={['*', 'auto']} 
                margin='5,5,5,5' >
                
                <Xaml.TextBlock grid$row={0} fontSize={28} text="My Todo List" margin='0,0,0,10' />
                <Xaml.TextBox
                    grid$row={2}
                    grid$column={0}
                    placeholderText='What do you need to do?'
                    onTextChanged={textChanged}
                    margin='0,0,5,0'
                    text={host.getState().inputValue}  />
                <Xaml.Button
                    grid$row={2}
                    grid$column={1}
                    content='add'
                    margin='0,0,0,0'
                    onClick={addTodo} />
                <Xaml.ListBox
                    grid$row={1}
                    grid$column={0}
                    grid$columnSpan={2}
                    margin='0,0,0,5'
                    itemsSource={host.getState().todos.map(renderTodo)}
                    />
            </Xaml.Grid>
        );
    }
}