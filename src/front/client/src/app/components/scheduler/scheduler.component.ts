import { Component, OnInit } from '@angular/core';
import { DeviceSchedulerService } from 'src/app/services/device-scheduler.service';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';

@Component({
  selector: 'app-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.css']
})
export class SchedulerComponent implements OnInit {

  constructor( private deviceService: DeviceService,
               private jwtService: JWTService,
               private deviceSchedulerService: DeviceSchedulerService) {

  }

  devices: any[] = [];

  ngOnInit() {
    this.deviceService.getDevicesIdAndNameByUserId( this.jwtService.userId).subscribe( (result:any) => {
      if( result.success) {
        this.devices = result.data;
      }
    }, error => {
      console.log( error);
    })
  }

  showForm: boolean = false;

}
