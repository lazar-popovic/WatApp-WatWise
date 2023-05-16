import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { User } from 'src/app/Models/User';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-delete-profile',
  templateUrl: './delete-profile.component.html',
  styleUrls: ['./delete-profile.component.css']
})
export class DeleteProfileComponent {
  @Output() output = new EventEmitter<boolean>();
  @Input() id: number = 0;
  @Input() user: User | undefined;
  busy: Subscription | undefined;
  constructor(private userService: UserService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  deleteProfile() {
    this.busy = this.userService.deleteUser(this.id).subscribe((result: any) => {
      console.log( result);
      if( result.body.success) {
        if(result.body.data == "User has been successfully deleted!")
          this.router.navigate(['/dso/prosumers']);
        else
          this.router.navigate(['/dso/employees']);
      } else {
        this.toastrNotifService.showSuccess( result.body.errors);
      }
    }, (error: any) => {
      console.log(error);
  });
  }

  hideForm() {
    this.output.emit(true);
  }
}
