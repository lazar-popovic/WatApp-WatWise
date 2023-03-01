import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Actors } from 'src/app/models/actors';
import { ActorsService } from 'src/app/services/actors.service';


@Component({
  selector: 'app-edit-actor',
  templateUrl: './edit-actor.component.html',
  styleUrls: ['./edit-actor.component.css']
})
export class EditActorComponent implements OnInit {
  @Input() actor?: Actors;
  @Output() actorsUpdated = new EventEmitter<Actors[]>();


  constructor(private actorsService: ActorsService) { }

  ngOnInit(): void {
  }

  updateActor(actor:Actors){
    this.actorsService.updateActors(actor).subscribe((actors: Actors[]) => this.actorsUpdated.emit(actors));
  }

  deleteActor(actor:Actors){
    this.actorsService.deleteActors(actor).subscribe((actors: Actors[]) => this.actorsUpdated.emit(actors));
  }

  createActor(actor:Actors){
    this.actorsService.createActors(actor).subscribe((actors: Actors[]) => this.actorsUpdated.emit(actors));
  }
}
