import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-device-schedule-card',
  templateUrl: './device-schedule-card.component.html',
  styleUrls: ['./device-schedule-card.component.css']
})
export class DeviceScheduleCardComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @Input() deviceId: number = 0;
}
