import { Component } from '@angular/core';
import { User } from 'src/app/Models/User';

@Component({
  selector: 'app-users-overview',
  templateUrl: './users-overview.component.html',
  styleUrls: ['./users-overview.component.css']
})
export class UsersOverviewComponent {

  users: User[] = [];
  currentIndex = 9;
  pageDist = 2;
  pagesNum = [1,2,3,4,5,6,7,8,9,10];

  dynamicList: number[] = [];

  constructor() {
    for(var i = 0; i < 10; i++) {
      this.users.push(new User());
    }
    this.makeList();
  }

  makeList() {
    this.dynamicList.push(1,2);
    if(this.currentIndex < this.pageDist*2+1) {
      for(var i = 3;i<this.pageDist*2+1;i++) {
        this.dynamicList.push(i);
      }
    }
    else if(this.currentIndex >= this.pageDist *2) {
      for(var i = this.currentIndex - this.pageDist;i<this.currentIndex + this.pageDist + 1;i++) {
        if(i <= this.pagesNum.length - this.pageDist)
          this.dynamicList.push(i);
      }
    }
    else {
      for(var i = this.pagesNum.length;i>this.pagesNum.length - this.pageDist*2 - 1;i--) {
        this.dynamicList.push(i);
      }
    }
    this.dynamicList.push(this.pagesNum.length-1,this.pagesNum.length);
  }
}
