import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css']
})
export class SliderComponent {
  @Output() sliderEvent = new EventEmitter<boolean>();

  emitData(status: boolean) {
    this.sliderEvent.emit(status);
  }

  click(event: MouseEvent) {
    this.emitData((event.target as HTMLInputElement).checked);
  }
}
