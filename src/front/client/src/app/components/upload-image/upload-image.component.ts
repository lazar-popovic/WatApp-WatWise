import { Component } from '@angular/core';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { AuthService } from 'src/app/services/auth-service.service';
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

  constructor(private userService: UserService, private authService: AuthService) { }

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
      this.data.id = this.authService.userId;
      this.data.imagePicture = this.croppedImage.split("base64,")[1];
      this.userService.uploadImage(this.data).subscribe((result :any) => {
      }, (error: any) => {

      })
    }
}
