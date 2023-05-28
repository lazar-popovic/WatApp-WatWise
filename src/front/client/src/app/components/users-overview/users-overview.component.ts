// import { Component } from '@angular/core';
// import { User } from 'src/app/Models/User';
// import { UserService } from 'src/app/services/user.service';

import { Component } from '@angular/core';

import { User } from '../../Models/User';
import {UserService} from '../../services/user.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-users-overview',
  templateUrl: './users-overview.component.html',
  styleUrls: ['./users-overview.component.css']
})
export class UsersOverviewComponent {

  //users: User[] = [];
  users: any[] = [];
  currentIndex = 1;
  pagesNum: number = 1;
  pageSize: any = 10;
  showAddUProsumer: boolean = false;

  usersData: any[] = []; // Variable to hold the user data
  columns: any[] = []; // Variable to hold the column names
  columnLabels: any[] = [];

  filter : any = {
    name : '',
    address: '',
    order: 'asc'
  };

  constructor(private userService: UserService, private router: Router) {
    //this.getNumberOfPages();
    this.getProsumers();
  }

  getProsumers() {
    //this.userService.getProsumers(this.pageSize, this.currentIndex, this.filter.name, this.filter.address, this.filter.order).subscribe((result: any) => {
      this.userService.getProsumersWithEnergyUsage().subscribe((result: any) => {
      this.usersData = [];
      for(let item of result.data) {
        let user: any = {
          id:item.id,
          firstname: item.firstname,
          lastname: item.lastname,
          address: item.location?.address,
          number: item.location?.addressNumber,
          city: item.location?.city,
          currentConsumption: item.currentConsumption,
          currentProduction: item.currentProduction,
          activeConsumers: item.consumingDevicesTurnedOn,
          activeProducers: item.producingDevicesTurnedOn,
        };

        this.usersData.push(user);
      }
      this.columns = Object.keys(this.usersData[0]).map(column => ({
        field: column,
        header: this.formatHeader(column)}));
        console.log(this.columns);
    },(error: any) => {
      console.log(error);
    });
  }

  formatHeader(column: string): string {
    const formattedColumn = column.replace(/([A-Z])/g, ' $1').trim();
    const firstLetterCapitalized = formattedColumn.charAt(0).toUpperCase() + formattedColumn.slice(1).toLowerCase();
    return firstLetterCapitalized;
  }


  formEmitter( event: boolean) : void {
    if( event == true) {
      this.getProsumers();
    }
    this.showAddUProsumer=false
  }
}
