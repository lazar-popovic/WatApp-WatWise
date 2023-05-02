import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DeviceService } from 'src/app/services/device.service';
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
    capacity: null,
    dataShare: false,
    currentUsage: null
  }

  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor( private deviceService: DeviceService, private toastrNotifService: ToastrNotifService) { }

  ngOnInit() {
  }

  onSliderChange( value: boolean) {
    this.output.emit( true);
    this.device.activityStatus = value
    console.log( value);

    this.deviceService.patchDeviceActivityStatus( this.device.id, value).subscribe(
      (result: any) => {
        if( result.body.success) {
          this.toastrNotifService.showSuccess( result.body.data.message);
          this.device.activityStatus = value;
        }
      }
    );
  }
}
