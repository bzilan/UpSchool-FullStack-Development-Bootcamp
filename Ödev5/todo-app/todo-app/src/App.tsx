import React, {useState} from 'react';
import {  Button, Input,Table  } from 'semantic-ui-react';

interface item {
  id: number;
  task: string;
  isCompleted: boolean;
  createdDate: Date;
}

function App(){
    const [todos, setTodos] = useState<item[]>([  
      { id: 1, task: "Learn Typescript with Betül", isCompleted: false, createdDate: new Date },
      { id: 2, task: "watch a movie", isCompleted: true, createdDate: new Date },
    ]);
    const [newTodo, setNewTodo] = useState("");
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
      const value = event.target.value;
      setNewTodo(value.trimStart());
    };


    const handleAdd = () => {
      if (newTodo) {
        const todo = {id: todos.length + 1,task: newTodo,isCompleted: false,createdDate: new Date(),};
        setTodos([...todos, todo]);
        setNewTodo("");
      }
    };
  

    const handleToggleComplete = (id: number) => {
      setTodos((prevTodos) =>
      prevTodos.map((todo: any) => {
        if (todo.id === id) {
          return {...todo,isCompleted: !todo.isCompleted,};
        }
          return todo;
        })
      );
    };

    const handleDelete = (id: number) => {
      const updatedTodos = todos.filter((todo) => todo.id !== id);
      setTodos(updatedTodos);
    };

    const handleDoubleClick = (id: number): void => {
      const updatedTodos = todos.map((todo) => {
        if (todo.id === id) {
          return {
            ...todo,
            isCompleted: !todo.isCompleted,
          };
        }
        return todo;
      });
      setTodos(updatedTodos);
    };

    const handleSortByDate = () => {
      if (sortOrder === "asc") {
        const sortedTodos = [...todos].sort(
            (a, b) => new Date(a.createdDate).getTime() - new Date(b.createdDate).getTime()
        );
        setTodos(sortedTodos);
        setSortOrder("desc");
      } else {
        const sortedTodos = [...todos].sort(
            (a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
        );
        setTodos(sortedTodos);
        setSortOrder("asc");
      }
    };


    return (
      <div className="main-container" >
       <h1>Todos</h1>
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        
        <Input
              className="todo-input"
              value={newTodo}
              onChange={handleInputChange}
              placeholder="What is the task today?"
          />
          <Button
              className="todo-button" primary onClick={handleAdd} disabled={!newTodo}>
            Add
          </Button>
          <Button.Group floated="right">
            {sortOrder === "asc" ? (
                <>
                  <Button content="oldest" onClick={handleSortByDate} />
                </>
            ) : (
                <>
                  <Button content="latest" onClick={handleSortByDate} />
                </>
            )}
          </Button.Group>
        </div>
        <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Task</Table.HeaderCell>
            <Table.HeaderCell>Date</Table.HeaderCell>
            <Table.HeaderCell>Actions</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {todos.map((todo) => (
            <Table.Row
              key={todo.id}
              style={{ textDecoration: todo.isCompleted ? 'line-through' : 'none', cursor: 'pointer' }}
              onDoubleClick={() => handleDoubleClick(todo.id)}
            >
              <Table.Cell>{todo.task}</Table.Cell>
              <Table.Cell>{todo.createdDate.toLocaleString()}</Table.Cell>
              <Table.Cell>
              <Button icon="check" content="Complete" onClick={() => handleToggleComplete(todo.id)} />
                <Button icon="delete" content="Delete" onClick={() => handleDelete(todo.id)} />
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </div>
  );
}


export default App;