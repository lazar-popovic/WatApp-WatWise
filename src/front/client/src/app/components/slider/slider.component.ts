import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges, OnChanges } from '@angular/core';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css']
})
export class SliderComponent implements OnInit, OnChanges{
  @Output() sliderEvent = new EventEmitter<boolean>();
  @Input() status = true;

  ngOnInit(): void {
    (document.querySelector('#slider-inp') as HTMLInputElement).checked = this.status;
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['status']) {
      const currentValue = changes['status'].currentValue;
      (document.querySelector('#slider-inp') as HTMLInputElement).checked = currentValue;
    }
  }

  emitData(status: boolean) {
    this.sliderEvent.emit(status);
  }

  click(event: MouseEvent) {
    this.emitData((event.target as HTMLInputElement).checked);
  }

  constructor() {}
}
