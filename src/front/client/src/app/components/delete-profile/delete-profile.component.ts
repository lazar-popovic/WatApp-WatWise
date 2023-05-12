import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-delete-profile',
  templateUrl: './delete-profile.component.html',
  styleUrls: ['./delete-profile.component.css']
})
export class DeleteProfileComponent {
  @Output() exitStatusEvent = new EventEmitter<boolean>();
  @Input() id: number = 0;

  constructor(private userService: UserService, private router: Router, private toastrNotifService: ToastrNotifService) { }

  deleteProfile() {
    this.userService.deleteUser(this.id).subscribe((result: any) => {
      console.log( result);
      if( result.body.success) {
        this.router.navigate(['/dso/prosumers']);
      } else {
        this.toastrNotifService.showSuccess( result.body.errors);
      }
    }, (error: any) => {
      console.log(error);
  });
  }

  hideForm() {
    this.exitStatusEvent.emit(true);
  }
}