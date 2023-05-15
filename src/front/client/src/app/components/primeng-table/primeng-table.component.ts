import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-primeng-table',
  templateUrl: './primeng-table.component.html',
  styleUrls: ['./primeng-table.component.css']
})
export class PrimengTableComponent {
  @Input() data: any[] | undefined;
  @Input() columns: string[] | undefined;

  ngOnInit(): void {
    if (this.data && this.data.length > 0) {
      this.columns = Object.keys(this.data[0]);
    }
  }
}
