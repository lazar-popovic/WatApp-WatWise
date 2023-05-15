import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css']
})
export class AddEmployeeComponent {
  employee: any = {
    email: '',
    firstname: '',
    lastname: '',
  };
  busy: Subscription | undefined;
  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();
  constructor(private userService: UserService,private router: Router, private toastrNotifService: ToastrNotifService) { }

  storeEmployee()
  {
    this.busy = this.userService.createEmployee(this.employee).subscribe((result: any) => {
      if(result.body.success) {
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
    let thisComponent = document.querySelector('.employee-overview-overlay') as HTMLDivElement;
    thisComponent.style.display = "none";
  }
}
