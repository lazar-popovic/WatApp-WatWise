import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'jquery';
import { Subscription } from 'rxjs';
import { DeviceSchedulerService } from 'src/app/services/device-scheduler.service';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-scheduler',
  templateUrl: './scheduler.component.html',
  styleUrls: ['./scheduler.component.css']
})
export class SchedulerComponent implements OnInit {

  constructor( private deviceService: DeviceService,
               private jwtService: JWTService,
               private deviceSchedulerService: DeviceSchedulerService,
               private toastrNotifService: ToastrNotifService,
               private router: Router) {

  }

  devices: any[] = [];
  selectedDevice: any = 0;

  routines: any[] = [];
  routinesDisplay: any[] = [];
  selectedRoutine: any = null;
  showForm: boolean = false;

  showDialog: boolean = false;

  busy: Subscription | undefined;

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

  showRoutine( routine:any) : void {
    this.selectedRoutine = routine;
    console.log( routine);
  }

  closeRoutine( ) : void {
    this.selectedRoutine = null;
  }

  deleteRoutine( id: number) : void {
    this.showDialog = false;
    console.log(id);
    if( id > 0) {
      this.busy = this.deviceSchedulerService.removeJob( id).subscribe((result:any) => {
        if( result.body.success) {
          this.toastrNotifService.showSuccess( result.body.data);
          this.router.navigate(["/prosumer/scheduler"]);
        }
        else {
          this.toastrNotifService.showErrors( result.body.errors);
        }
      }, (erros:any) => {
        console.log( error);
      })
    }
  }
}
