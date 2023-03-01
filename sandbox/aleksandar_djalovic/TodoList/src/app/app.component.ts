import { Component } from '@angular/core';
import { Todo } from './models/todo';
import { TodoService } from './Services/todo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TodoList';
  todos: Todo[] = [];

  constructor(private todoService: TodoService) {}

  ngOnInit() : void {
    this.todoService.getTodos().subscribe((result: Todo[]) => (this.todos = result));
  }
}
