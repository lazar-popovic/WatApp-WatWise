import { Component } from '@angular/core';
import { event } from 'jquery';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-employee-overview',
  templateUrl: './employee-overview.component.html',
  styleUrls: ['./employee-overview.component.css']
})
export class EmployeeOverviewComponent {
  users: User[] = [];
  currentIndex = 1;
  pagesNum: number = 1;
  pageSize: any = 10;
  showPrompt: boolean = false;
  showAddNewEmployeeForm: boolean = false;
  filter : any = {
    name: '',
    sortOrder: '',
    mail: ''
  }
  showDelete: boolean = false;
  showResend: boolean = false;
  selectedUser: any = null;

  usersData: any[] = []; // Variable to hold the user data
  columns: any[] = []; // Variable to hold the column names

  constructor(private userService: UserService) {
    this.getEmployees();
  }

  getEmployees() {
    this.userService.getEmployees(this.filter.name, this.filter.sortOrder, this.pageSize, this.pagesNum).subscribe((result: any) => {
      this.users = [];
      for(let item of result.data) {
        let user = new User();
        user.firstName = item.firstname;
        user.lastName = item.lastname;
        user.id = item.userId;
        user.mail = item.email;
        user.roleId = item.roleId;
        this.users.push(user);
      }
    },(error: any) => {
      console.log(error);
    });
  }

  addEmployee() {
    this.showAddNewEmployeeForm = true
  }

  getNumberOfPages() {
    //this.userService.getNumberOfEmployees();
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

    active.innerHTML = this.currentIndex as unknown as string;
  }

  pageSizeHandler() {
    console.log(this.pageSize);
  }

  formEmitter( event: boolean) : void {
    if( event == true) {
      this.getEmployees();
    }
    this.showAddNewEmployeeForm = false
  }

  showResendForm( user: any) {
    this.selectedUser = user;
    console.log( this.selectedUser);
    this.showResend = true
  }

  showDeleteForm( user: any) {
    this.selectedUser = user;
    console.log( this.selectedUser);
    this.showDelete = true
  }
}
