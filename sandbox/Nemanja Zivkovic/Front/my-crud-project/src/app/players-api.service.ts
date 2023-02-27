import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlayersApiService {

  readonly playersAPIUrl = "https://localhost:44350/api";

  constructor(private http:HttpClient) { }

  getPlayersList():Observable<any[]>{
    return this.http.get<any>(this.playersAPIUrl + '/Players');
  }

  addPlayer(data:any){
    return this.http.post(this.playersAPIUrl + '/Players', data);
  }

  updatePlayer(id:number, data:any){
    return this.http.put(this.playersAPIUrl + `/Players/${id}`, data);
  }

  deletePlayer(id:number){
    return this.http.delete(this.playersAPIUrl + `/Players/${id}`);
  }

  getTeamsList():Observable<any[]>{
    return this.http.get<any>(this.playersAPIUrl + '/Teams');
  }
}
