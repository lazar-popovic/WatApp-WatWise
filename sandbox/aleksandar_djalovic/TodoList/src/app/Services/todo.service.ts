import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Todo } from '../models/todo';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  url = "Todo";

  constructor(private http: HttpClient) { }

  public getTodos() : Observable<Todo[]> {
    return this.http.get<Todo[]>(`${environment.apiUrl}/${this.url}`);
    /*let todo = new Todo();
    todo.id = 1;
    todo.title = "a";
    todo.description = "b";
    return [todo];*/
  }
}
