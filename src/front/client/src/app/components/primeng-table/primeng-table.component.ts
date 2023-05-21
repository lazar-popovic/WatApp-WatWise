import { Component, Input, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import * as FileSaver from 'file-saver';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { HtmlParser } from '@angular/compiler';
import { WorkSheet } from 'xlsx';
import * as saveAs from 'file-saver';


@Component({
  selector: 'app-primeng-table',
  templateUrl: './primeng-table.component.html',
  styleUrls: ['./primeng-table.component.css']
})
export class PrimengTableComponent{

  @ViewChild('dt1') dt1: Table | undefined;
  @Input() tableData: any[] = [];
  @Input() columns: any[] = [];
  @Input() columnLabels: any[] = [];
  loading: boolean = true;
  selectedData: any[] = [];
  exportColumns: any[] = [];

  ngOnInit() {
    this.loading = true;
    if (this.tableData && this.tableData.length > 0) {
      this.columns = Object.keys(this.tableData[0]);
  }
}

  isColumnNumber(column: any): boolean {
    const firstItem = this.tableData[0];
    return typeof firstItem[column] === 'number';
  }

  clear(table: Table) {
    table.clear();
  }

  applyFilterGlobal($event: any, stringVal: any) {
    this.dt1!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
  }


  exportPdf() {
    import('jspdf').then((jsPDF) => {
        import('jspdf-autotable').then((x) => {
            const doc = new jsPDF.default('l', 'px', 'a4');
            const rows = this.tableData.map(item => this.columns.map(col => item[col]));

            (doc as any).autoTable({
              head: [this.columnLabels],
              body: rows,
            });
              doc.save('Data.pdf');
          });
    });
}

exportExcel() {
    import('xlsx').then((xlsx) => {
        const dataWithHeaders = [];
        dataWithHeaders.push(this.columnLabels);

        // Add data rows
        for (const item of this.tableData) {
          const dataRow = this.columns.map(col => item[col]);
          dataWithHeaders.push(dataRow);
        }

        const worksheet = xlsx.utils.aoa_to_sheet(dataWithHeaders);
        const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
        const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
        this.saveAsExcelFile(excelBuffer, 'Data');
    });
}

saveAsExcelFile(buffer: any, fileName: string): void {
    let EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    let EXCEL_EXTENSION = '.xlsx';
    const data: Blob = new Blob([buffer], {
        type: EXCEL_TYPE
    });
    FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
}

}
