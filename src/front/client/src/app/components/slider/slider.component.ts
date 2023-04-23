import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css']
})
export class SliderComponent implements OnInit{
  @Output() sliderEvent = new EventEmitter<boolean>();
  @Input() status = true;

  ngOnInit(): void {
    (document.querySelector('#slider-inp') as HTMLInputElement).checked = this.status;
  }

  emitData(status: boolean) {
    this.sliderEvent.emit(status);
  }

  click(event: MouseEvent) {
    this.emitData((event.target as HTMLInputElement).checked);
  }
}
