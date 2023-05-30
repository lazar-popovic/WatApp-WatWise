import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-profile-map',
  templateUrl: './profile-map.component.html',
  styleUrls: ['./profile-map.component.css']
})
export class ProfileMapComponent implements OnInit, OnChanges {

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
  }

  ngOnInit() {
  }

}
