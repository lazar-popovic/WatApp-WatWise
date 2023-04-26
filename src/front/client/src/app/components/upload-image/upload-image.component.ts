import { Component } from '@angular/core';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent {
  imageSize: number = 200;
  imageChangedEvent: any = '';
    croppedImage: any = '/assets/profile.jpeg';

  constructor(private userService: UserService) { }

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
      console.log(this.croppedImage);
      let image = this.croppedImage.split("base64,")[1];
      this.userService.uploadImage(image).subscribe((result :any) => {

      }, (error: any) => {

      })
    }
}
