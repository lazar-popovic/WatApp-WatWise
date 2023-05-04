import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { DeviceSchedulerService } from 'src/app/services/device-scheduler.service';

@Component({
  selector: 'app-device-schedule-card',
  templateUrl: './device-schedule-card.component.html',
  styleUrls: ['./device-schedule-card.component.css']
})
export class DeviceScheduleCardComponent implements OnInit, OnChanges {

  constructor( private deviceSchedulerService: DeviceSchedulerService) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges) {
    const id = changes['deviceId'].currentValue;
    if( id != 0) {
      this.getActiveJobForDeviceId( id);
    }
  }

  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  busy: Subscription | undefined;

  @Input() deviceId: number = 0;

  deviceJob = {
    id: 0,
    startDate: "",
    endDate: "",
    turn: false,
    repeat: false
  };

  getActiveJobForDeviceId( id: number) {
    this.busy = this.deviceSchedulerService.getActiveJobForDeviceId( id).subscribe((result:any) => {
      console.log( result.data);
      this.deviceJob = result.data;
    }, (error:any) => {
      console.log( error);
    });
  }

  outputEmit() {
    this.output.emit( true);
    console.log( true);
  }
}
