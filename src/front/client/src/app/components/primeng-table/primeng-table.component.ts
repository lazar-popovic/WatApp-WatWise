import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-primeng-table',
  templateUrl: './primeng-table.component.html',
  styleUrls: ['./primeng-table.component.css']
})
export class PrimengTableComponent {
  @Input() tableData: any[] = [];
  @Input() columns: any[] = [];
  @Input() columnLabels: any[] = [];

  ngOnInit(): void {
    if (this.tableData && this.tableData.length > 0) {
      this.columns = Object.keys(this.tableData[0]);
    }
  }
}
