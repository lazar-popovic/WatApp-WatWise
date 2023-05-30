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
      if( result.body.success) {
        console.log( this.user);
        this.toastrNotifService.showSuccess( result.body.data);
        if( this.user?.roleId == 3) {
          this.router.navigate(['/dso/prosumers']);
        }
        else {
          window.location.reload();
        }
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
