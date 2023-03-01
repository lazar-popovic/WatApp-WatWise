import { Component } from '@angular/core';
import { Actors } from './models/actors';
import { ActorsService } from './services/actors.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Actors';
  actors: Actors[] = [];
  actorToEdit?: Actors;

  constructor(private actorsService: ActorsService) {}
  ngOnInit() : void {
    this.actorsService.getActors().subscribe((result: Actors[]) => (this.actors = result));
  }

  updateActorsList(actors: Actors[])
  {
    this.actors = actors;
  }

  initNewActor(){
    this.actorToEdit = new Actors();
  }

  editActor(actor: Actors) {
    this.actorToEdit = actor;
  }
}
