import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PlayersApiService } from 'src/app/players-api.service';

@Component({
  selector: 'app-show-player',
  templateUrl: './show-player.component.html',
  styleUrls: ['./show-player.component.css']
})
export class ShowPlayerComponent implements OnInit {

  playersList$!:Observable<any[]>;
  teamsList$!:Observable<any[]>; //za sada ne koristim
  teamsList:any = [];

  teamsMap:Map<number, string> = new Map();

  constructor(private service:PlayersApiService) { }

  ngOnInit(): void {
    
    this.playersList$ = this.service.getPlayersList();
    this.teamsList$ = this.service.getTeamsList();
    this.refreshTeamsMap();
  }

  activateAddEditPlayerComponent:boolean = false;
  player: any;
  modalTitle: string = '';

  refreshTeamsMap(){

    this.service.getTeamsList().subscribe(data => {

      this.teamsList = data;

      for(let i = 0; i < data.length; i++){
        this.teamsMap.set(this.teamsList[i].id, this.teamsList[i].teamName);
      }

    })
  }

  modalAdd(){

    this.player = {
      id:0,
      playerName:null,
      playerNumber:null,
      teamId:null,
      team:null
    }
    this.modalTitle = "Add new player";
    this.activateAddEditPlayerComponent = true;
  }

  modalEdit(item:any){

    this.player = item;
    this.modalTitle = "Edit existing player";
    this.activateAddEditPlayerComponent = true;
  }

  modalClose(){

    this.activateAddEditPlayerComponent = false;
    this.playersList$ = this.service.getPlayersList(); //kad se modal zatvori, tabela se opet cita i biva popunjena novim podacima
  }

  delete(item: any){
    if(confirm(`Are you sure?`)){
      this.service.deletePlayer(item.id).subscribe(res => {
        var closeModalButton = document.getElementById('add-edit-modal-close');
      if(closeModalButton){
        closeModalButton.click();
      } 

      var showDeleteSuccess = document.getElementById('delete-success-alert');
      if(showDeleteSuccess){
        showDeleteSuccess.style.display = "block";
      }
      setTimeout(function(){
        if(showDeleteSuccess){
          showDeleteSuccess.style.display = "none";
        }
      }, 4000);
      this.playersList$ = this.service.getPlayersList();
      })
    }
  }
}
