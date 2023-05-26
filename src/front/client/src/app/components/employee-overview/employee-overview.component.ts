import { Component } from '@angular/core';
import { event } from 'jquery';
import { Subscription } from 'rxjs';
import { User } from 'src/app/Models/User';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
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
  busyDelete: Subscription | undefined;


  constructor(private userService: UserService, private toastrNotifService: ToastrNotifService) {
    this.getEmployees();
  }

  getEmployees() {
    this.userService.getEmployees().subscribe((result: any) => {
      this.usersData = [];
      for(let item of result.data) {
        let user: any = {
          firstname: item.firstname,
          lastname: item.lastname,
          mail: item.email,
          id: item.id
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

  addEmployee() {
    this.showAddNewEmployeeForm = true
  }

  formEmitter( event: boolean) : void {
    if( event == true) {
      this.getEmployees();
    }
    this.showAddNewEmployeeForm = false
  }

  showResendForm( rowData: any) {
    const userEmail = rowData.mail;
    console.log( userEmail);
    this.showResend = true
  }

  showDeleteForm( rowData: any) {
    const userId = rowData.id;
    console.log( userId);
    this.showDelete = true
  }

  deleteEmployee(id: any)
  {
    this.busyDelete = this.userService.deleteUser(id).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess("Login successful!");
      }
      else {
        this.toastrNotifService.showErrors( result.body.errors);
      }
    },(error: any) => {
      console.log(error.error.errors)
    })
  }

  resendVerification(email: any)
  {

  }
}
