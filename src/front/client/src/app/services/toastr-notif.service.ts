import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastrNotifService {
  constructor(private toastr: ToastrService) {}
  showErrors(errors: string[]) {

    // SVAKA PORUKA ZASEBNI NOTIF
    for (let i = 0; i < errors.length; i++) {
      this.toastr.error(errors[i], 'Error', {
        positionClass: 'toast-bottom-center'
      });
    }

    // JEDAN NOTIF ZA SVE PORUKE
    /*let message = "";
    for (let i = 0; i < errors.length; i++) {
      message = message + errors[i] + "<br><br>";
    }
    this.toastr.error(message.trim(), 'Error', {
      positionClass: 'toast-bottom-center',
      enableHtml: true
    });*/
  }
}
