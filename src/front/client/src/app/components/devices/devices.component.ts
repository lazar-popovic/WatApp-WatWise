import { Component } from '@angular/core';
import { Device } from '../../Models/device';
import {DeviceService} from "../../services/device.service";
import {JWTService} from "../../services/jwt.service";
import {ToastrNotifService} from "../../services/toastr-notif.service";
import {Router} from "@angular/router";



@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent
{
    showAddDevice : boolean = false;

    newDevice: any = {
      userId: 1,
      deviceTypeId: 1,
      name: '',
    };

    constructor( private deviceService: DeviceService, private jwtService: JWTService, private toastrService: ToastrNotifService, private route: Router) { }

    addDevice()
    {
      this.newDevice.userId = this.jwtService.userId;
      this.deviceService.insertDevice( this.newDevice).subscribe(
        result => {
          if( result.body.success)
          {
            this.toastrService.showSuccess( result.body.data);
          }
          else
          {
            this.toastrService.showErrors( result.body.errors);
          }
        }
      )
    }

    refresh() {
      this.route.navigateByUrl('/prosumer/devices');
    }

    toDevice( id: number) {
      console.log( id);
      this.route.navigate(['/prosumer/device',id]);
    }
}

