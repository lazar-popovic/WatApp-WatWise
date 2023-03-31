import { Component } from '@angular/core';
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

  constructor(private userService: UserService) {
    this.getEmployees();
  }

  getEmployees() {
    this.userService.getEmployees(this.pageSize, this.pagesNum).subscribe((result: any) => {
      for(let item of result.data) {
        let user = new User();
        user.firstName = item.firstname; 
        user.lastName = item.lastname; 
        user.id = item.id;
        user.mail = item.email;
        this.users.push(user);
      }
    },(error: any) => {
      console.log(error);
    });
  }

  addEmployee() {
    
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
}
