import { Component, OnInit } from '@angular/core';
import { error } from 'jquery';
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
  selectedDevice: any = 0;

  routines: any[] = [];
  routinesDisplay: any[] = [];
  showForm: boolean = false;

  ngOnInit() {
    this.deviceService.getDevicesIdAndNameByUserId( this.jwtService.userId).subscribe( (result:any) => {
      if( result.success) {
        this.devices = result.data;
      }
    }, error => {
      console.log( error);
    })

    this.getRoutines( true);
  }


  getRoutines( active: boolean) : void {
    this.deviceSchedulerService.getJobsForUserId( this.jwtService.userId, active).subscribe((result:any) => {
      if( result.success) {
        this.routines = result.data;
        this.filter();
        console.log( this.routines);
      }
    }, (erros:any) => {
      console.log( error);
    })
  }

  formatDate( date: string) {
    let dateArray = date.split("T")[0].split("-");
    return(`${dateArray[2]}.${dateArray[1]}.${dateArray[0]}`)
  }

  filter() : void {
    this.routinesDisplay = this.routines.filter( (routine:any) => routine.deviceId == this.selectedDevice || this.selectedDevice == 0);
  }
}
