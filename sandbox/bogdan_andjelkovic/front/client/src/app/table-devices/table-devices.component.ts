import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DeviceService} from "../device.model/device.service";

@Component({
  selector: 'app-table-devices',
  templateUrl: './table-devices.component.html',
  styleUrls: ['./table-devices.component.css']
})
export class TableDevicesComponent implements OnInit{
  baseAPI = 'http://localhost:5029/devices';

  constructor(private http: HttpClient, public devicesService: DeviceService) {
  }
  getDevices() {
    this.http.get(this.baseAPI).subscribe( response => {
      this.devicesService.devices = response;
    }, error => {
      console.log( error);
    })
  }
  ngOnInit() {
    this.getDevices();
  }

  deleteDevice( id: number) {
    console.log(`${this.baseAPI}/${id}`);
    this.http.delete(`${this.baseAPI}/${id}`).subscribe(response => {
      this.devicesService.devices = this.devicesService.devices.filter( (device: { id: number; }) => device.id !== id);
      console.log('Response:', response);
    }, error => {
      console.log('Error:', error);
    });
  }

  updateDevice( id: number) {

  }
}
