import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.css']
})
export class SliderComponent implements OnInit{
  @Output() sliderEvent = new EventEmitter<boolean>();
  @Input() status = false;

  ngOnInit(): void {
    console.log( this.status);
  }

  emitData(status: boolean) {
    this.sliderEvent.emit(status);
  }

  click(event: MouseEvent) {
    this.emitData((event.target as HTMLInputElement).checked);
  }
}
