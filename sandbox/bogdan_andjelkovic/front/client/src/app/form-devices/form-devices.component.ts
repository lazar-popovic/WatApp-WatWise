import { Component } from '@angular/core';
import { Device} from "../device.model/device";
import {HttpClient} from "@angular/common/http";
import {DeviceService} from "../device.model/device.service";

@Component({
  selector: 'app-form-devices',
  templateUrl: './form-devices.component.html',
  styleUrls: ['./form-devices.component.css']
})

export class FormDevicesComponent {
  device: Device = new Device();
  baseAPI = 'http://localhost:5029/devices';
  name: any;
  price: any;
  constructor( private http: HttpClient, public devicesService: DeviceService) {
  }
  submitForm() {
    // set the device properties from the form
    console.log( this.name);
    console.log( this.price);
    this.device.Name = this.name;
    this.device.Price = Number(this.price);
    // send the device object via a POST request to the API
    this.http.post<Device>(`${this.baseAPI}?Name=${this.name}&Price=${Number(this.price)}`, this.device).subscribe(
      response => {
        this.devicesService.addDevice( response);
        console.log('POST response:', response);
      }, error => console.error('POST error:', error)
    );
  }
}
