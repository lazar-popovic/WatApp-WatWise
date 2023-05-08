import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-id-delete-dialog',
  templateUrl: './id-delete-dialog.component.html',
  styleUrls: ['./id-delete-dialog.component.css']
})
export class IdDeleteDialogComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @Input() text: string = "Are you sure you want to do that?";
  @Input() id: number = 0;
  @Output() output: EventEmitter<number> = new EventEmitter<number>();

  emit( value: boolean) {
    if( value) {
      this.output.emit( this.id);
    }
    else {
      this.output.emit( -1);
    }
  }
}
