import { Component } from '@angular/core';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { AuthService } from 'src/app/services/auth-service.service';
import { ToastrNotifService } from 'src/app/services/toastr-notif.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent {
  data: any = {
    id: 0,
    imagePicture: ''
  }
  imageSize: number = 200;
  imageChangedEvent: any = '';
  croppedImage: any = '/assets/profile.jpeg';

  constructor(private userService: UserService, private authService: AuthService, private toastrNotifService: ToastrNotifService) { }

    fileChangeEvent(event: any): void {
        this.imageChangedEvent = event;
    }
    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
    }
    imageLoaded() {
        // show cropper
    }
    cropperReady() {
        // cropper ready
    }
    loadImageFailed() {
        // show message
    }

    uploadImage() {
      if( this.croppedImage != '/assets/profile.jpeg') {
        this.data.id = this.authService.userId;
        this.data.imagePicture = this.croppedImage.split("base64,")[1];
        this.userService.uploadImage(this.data).subscribe((result :any) => {
          if( result.body.success) {
            this.toastrNotifService.showSuccess( result.body.data.message);
            window.location.reload();
          }
          else {
            this.toastrNotifService.showErrors( result.body.errors);
          }
        }, (error: any) => {

        })
      }
      else {
        this.toastrNotifService.showErrors(["Image not selected!"]);
      }
    }

    hideUpload() {
      (document.querySelector(".profile-overview-overlay") as HTMLDivElement).style.display = "none";
    }

}
