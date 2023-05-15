import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

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
    deviceType: { type: null, category: null },
    deviceSubtype: { subtypeName: null },
    capacity: 1,
    dataShare: false,
    currentUsage: 0,
    dsoControl: false
  }

  role: number = 3;
  disabled: boolean = false;

  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() outputEdit: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() outputDelete: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor( private deviceService: DeviceService,
               private toastrNotifService: ToastrNotifService,
               private jwtService: JWTService) { }

  ngOnInit() {
    this.role = this.jwtService.roleId;
  }

  onSliderChange( value: boolean) {
    this.output.emit( value);
  }
}
