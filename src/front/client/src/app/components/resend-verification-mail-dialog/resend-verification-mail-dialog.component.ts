import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth-service.service';
import { DeviceService } from 'src/app/services/device.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';

@Component({
  selector: 'app-resend-verification-mail-dialog',
  templateUrl: './resend-verification-mail-dialog.component.html',
  styleUrls: ['./resend-verification-mail-dialog.component.css']
})
export class ResendVerificationMailDialogComponent implements OnInit {
  @Output() exitStatusEvent = new EventEmitter<boolean>();
  @Input() email: string = "";

  busy: Subscription | undefined;

  constructor( private authService: AuthService, private router: Router, private toastrNotifService: ToastrNotifService) {
  }

  ngOnInit(): void {
  }

  deleteDevice() {
    this.busy = this.authService.resendVerificationMail( {email:this.email}).subscribe((result: any) => {
      if( result.body.success) {
        this.toastrNotifService.showSuccess( result.body.data.message);
        this.exitStatusEvent.emit( true);
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

