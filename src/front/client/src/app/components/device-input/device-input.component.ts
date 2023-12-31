import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DeviceService } from 'src/app/services/device.service';
import { JWTService } from 'src/app/services/jwt.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-device-input',
  templateUrl: './device-input.component.html',
  styleUrls: ['./device-input.component.css']
})
export class DeviceInputComponent implements OnInit{

  showDevices : boolean = false;

  @Input() showAddDevice = true;
  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  newDevice = {
    userId: 0,
    name: "",
    deviceTypeId: 0,
    category: -1,
    capacity: "",
    deviceSubtypeId: 0
  }
  category: number = -1;
  types: any[] = [];
  subtypes: any[] = [];

  userDevices = {
    consumers: [] as any[],
    storages: [] as any[],
    producers: [] as any[]
  }

  busyAddDevice: Subscription | undefined;

  constructor( private deviceService: DeviceService, private jwtService: JWTService, private toastrService: ToastrNotifService, private route: Router) { }

  ngOnInit(): void {
    this.fillTypes();
  }

  fillTypes() {
    this.deviceService.getDeviceTypesByCategory(this.newDevice.category).subscribe(
      result => {
        this.types = result.data;
        if(this.types.length > 0) {
          this.newDevice.deviceTypeId = this.types[0].id;
        }
        this.fillSubtypes();
      }, error => {
        console.log( error.errors);
      }
    )
  }

  fillSubtypes() {
    this.deviceService.getDeviceSubtypesByType(this.newDevice.deviceTypeId).subscribe(
      result => {
        this.subtypes = result.data;
        if(this.types.length > 0) {
          this.newDevice.deviceSubtypeId = this.subtypes[0].id;
        }
      }, error => {
        console.log( error.errors);
      }
    )
  }

  addDevice()
  {
    this.newDevice.userId = this.jwtService.userId;
    if( this.newDevice.capacity == "") {
      this.newDevice.capacity = "0";
    }
    this.busyAddDevice = this.deviceService.insertDevice( this.newDevice).subscribe(
      result => {
        if( result.body.success)
        {
          this.toastrService.showSuccess( result.body.data.message);
          this.route.navigateByUrl("prosumer/device/"+result.body.data.deviceId);
        }
        else
        {
          this.newDevice.capacity = "";
          this.toastrService.showErrors( result.body.errors);
        }
      }
    )
  }

  refresh() {
    this.output.emit(true);
  }
}
