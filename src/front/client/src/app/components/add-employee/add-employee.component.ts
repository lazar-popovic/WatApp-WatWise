import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent {
  user: any = {
    email: '',
    firstname: '',
    lastname: '',
  };

  constructor(private userService: UserService,private router: Router, private toastrNotifService: ToastrNotifService) { }

  storeUser()
  {
    this.userService.createUser(this.user).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess(result.body.data.message);
      }
      else {
        this.toastrNotifService.showErrors(result.body.errors);
      }
    },(error: any) => {
      console.log(error);
    });

  }
  
  hide() {
    let thisComponent = document.querySelector('.employee-add') as HTMLDivElement;
    thisComponent.style.display = "None";
  }
}
