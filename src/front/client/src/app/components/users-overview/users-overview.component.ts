// import { Component } from '@angular/core';
// import { User } from 'src/app/Models/User';
// import { UserService } from 'src/app/services/user.service';

import { Component } from '@angular/core';

import { User } from '../../Models/User';
import {UserService} from '../../services/user.service'

@Component({
  selector: 'app-users-overview',
  templateUrl: './users-overview.component.html',
  styleUrls: ['./users-overview.component.css']
})
export class UsersOverviewComponent {

  users: User[] = [];
  currentIndex = 1;
  pagesNum: number = 1;
  pageSize: any = 10;
  showAddUProsumer: boolean = false;

  filter : any = {
    name : '',
    address: '',
    order: 'asc'
  };

  constructor(private userService: UserService) {
    this.getNumberOfPages();
    this.getProsumers();
  }

  getProsumers() {
    this.userService.getProsumers(this.pageSize, this.currentIndex, this.filter.name, this.filter.address, this.filter.order).subscribe((result: any) => {
      this.users = [];
      for(let item of result.data) {
        let user = new User();
        user.firstName = item.firstname;
        user.lastName = item.lastname;
        user.id = item.id;
        user.mail = item.email;
        if (item.location != null) {
          user.address = item.location.address;
          user.num = item.location.addressNumber;
          user.city = item.location.city;
        }
        this.users.push(user);
      }
    },(error: any) => {
      console.log(error);
    });
  }

  handler(type: number) {
    let active = document.querySelector(".overview-pagination-page-active") as HTMLDivElement;

    if(type == 2) {
      if(this.currentIndex < this.pagesNum)
        this.currentIndex++;
    }
    if(type == 1  ) {
      if(this.currentIndex>1)
      this.currentIndex--;
    }
    this.getProsumers();
    active.innerHTML = this.currentIndex as unknown as string;
  }

  getNumberOfPages() {
    this.userService.getNumberOfProsumers().subscribe((result: any) => {
      this.pagesNum = Math.ceil((result.body.data as number)/this.pageSize);
    }, (error : any) => {
      console.log(error);
    });
  }

  pageSizeHandler() {
    this.getNumberOfPages();
    this.getProsumers();
  }

  inverseOrder() {
    if(this.filter.order == "asc")
      this.filter.order = "desc";
    else if(this.filter.order == "desc")
      this.filter.order = "asc";
    
    let element = document.querySelector("#lastname-filter") as HTMLDivElement;

    if(this.filter.order == "asc")
      element.innerText = "Last name ↑";
    if(this.filter.order == "desc")
      element.innerText = "Last name ↓";

      this.getProsumers();
  }
}