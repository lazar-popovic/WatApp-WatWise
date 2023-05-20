import { Component, Input, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-primeng-table',
  templateUrl: './primeng-table.component.html',
  styleUrls: ['./primeng-table.component.css']
})
export class PrimengTableComponent {

  @ViewChild('dt1') dt1: Table | undefined;
  @Input() tableData: any[] = [];
  @Input() columns: any[] = [];
  @Input() columnLabels: any[] = [];

  ngOnInit(): void {
    if (this.tableData && this.tableData.length > 0) {
      this.columns = Object.keys(this.tableData[0]);
    }
  }

  isColumnNumber(column: string): boolean {
    const firstItem = this.tableData[0];
    return typeof firstItem[column] === 'number';
  }

  clear(table: Table) {
    table.clear();
}

applyFilterGlobal($event: any, stringVal: any) {
  this.dt1!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
}
}
