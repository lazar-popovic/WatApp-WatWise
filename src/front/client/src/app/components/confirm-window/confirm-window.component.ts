import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-confirm-window',
  templateUrl: './confirm-window.component.html',
  styleUrls: ['./confirm-window.component.css']
})
export class ConfirmWindowComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @Input() text: string = "Are you sure you want to do that?";

  @Output() output: EventEmitter<boolean> = new EventEmitter<boolean>();

  emitYes() {
    this.output.emit( true);
    console.log( true);
  }

  emitNo() {
    this.output.emit( false);
    console.log( false);
  }
}
