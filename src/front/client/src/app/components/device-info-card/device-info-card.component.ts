import { Component, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-device-info-card',
  templateUrl: './device-info-card.component.html',
  styleUrls: ['./device-info-card.component.css']
})
export class DeviceInfoCardComponent implements OnInit {
  @Input() device = {
    id: 0,
    userId: 0,
    name: "",
    activityStatus: false,
    deviceType: { type: null },
    deviceSubtype: { subtypeName: null },
    capacity: null,
    dataShare: false
  }
  @Output() sliderOutput = null;
  constructor() { }

  ngOnInit() {
  }

}
