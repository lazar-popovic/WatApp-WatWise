import { Component } from '@angular/core';
import { User } from 'src/app/Models/User';
import { UserService } from 'src/app/services/user.service';

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

  constructor(private userService: UserService) {
    this.getProsumers();
  }

  getProsumers() {
    this.userService.getProsumers(this.pageSize, this.pagesNum).subscribe((result: any) => {
      for(let item of result.data) {
        let user = new User();
        user.firstName = item.firstname; 
        user.lastName = item.lastname; 
        user.id = item.id;
        user.mail = item.email;
        user.address = item.location;
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

    active.innerHTML = this.currentIndex as unknown as string;
  }

  pageSizeHandler() {
    console.log(this.pageSize);
  }
}
