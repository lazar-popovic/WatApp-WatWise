import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PlayersApiService } from 'src/app/players-api.service';

@Component({
  selector: 'app-add-edit-player',
  templateUrl: './add-edit-player.component.html',
  styleUrls: ['./add-edit-player.component.css']
})
export class AddEditPlayerComponent implements OnInit {

  playersList$!: Observable<any[]>;
  teamsList$!: Observable<any[]>;

  constructor(private service:PlayersApiService) { }

  @Input() player:any;
  id: number = 0;
  playerName: string = "";
  playerNumber: number = 0;
  teamId: number = 0; //DRUGACIJE


  ngOnInit(): void {
    this.id = this.player.id;
    this.playerName = this.player.playerName;
    this.playerNumber = this.player.playerNumber;
    this.teamId = this.player.teamId;
    this.playersList$ = this.service.getPlayersList();
    this.teamsList$ = this.service.getTeamsList();
  }

  addPlayer(){

    var player = {
      playerName:this.playerName,
      playerNumber:this.playerNumber,
      teamId:this.teamId
    }
    this.service.addPlayer(player).subscribe(res => {
      var closeModalButton = document.getElementById('add-edit-modal-close');
      if(closeModalButton){
        closeModalButton.click();
      }

      var showAddSuccess = document.getElementById('add-success-alert');
      if(showAddSuccess){
        showAddSuccess.style.display = "block";
      }
      setTimeout(function(){
        if(showAddSuccess){
          showAddSuccess.style.display = "none";
        }
      }, 4000);
    })
  }

  updatePlayer(){

    var player = {
      id: this.id,
      playerName:this.playerName,
      playerNumber:this.playerNumber,
      teamId:this.teamId
    }

    var id:number = this.id;

    this.service.updatePlayer(id, player).subscribe(res => {
      var closeModalButton = document.getElementById('add-edit-modal-close');
      if(closeModalButton){
        closeModalButton.click();
      } 

      var showUpdateSuccess = document.getElementById('update-success-alert');
      if(showUpdateSuccess){
        showUpdateSuccess.style.display = "block";
      }
      setTimeout(function(){
        if(showUpdateSuccess){
          showUpdateSuccess.style.display = "none";
        }
      }, 4000);
    })
  }
}
