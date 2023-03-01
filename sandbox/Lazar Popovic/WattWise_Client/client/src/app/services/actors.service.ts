import { Injectable } from '@angular/core';
import { Actors } from '../models/actors';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class ActorsService {
  private url = "";

  constructor(private http: HttpClient) { }

  public getActors() : Observable<Actors[]>{
    return this.http.get<Actors[]>(`${environment.apiUrl}/${this.url}`);
  }

  public updateActors(actor: Actors) : Observable<Actors[]>{
    return this.http.put<Actors[]>(`${environment.apiUrl}/${this.url}`,actor);
  }

  public createActors(actor: Actors) : Observable<Actors[]>{
    return this.http.post<Actors[]>(`${environment.apiUrl}/${this.url}`,actor);
  }

  public deleteActors(actor: Actors) : Observable<Actors[]>{
    return this.http.delete<Actors[]>(`${environment.apiUrl}/${this.url}/${actor.id}`);
  }
}
