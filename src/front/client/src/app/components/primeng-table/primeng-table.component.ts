import { Component, Input, ViewChild } from '@angular/core';
import { Table } from 'primeng/table';
import * as FileSaver from 'file-saver';

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

  ngOnInit(): void {
    this.loading = true;
    if (this.tableData && this.tableData.length > 0) {
      this.columns = Object.keys(this.tableData[0]);
      this.exportColumns = this.columns.map((column) => ({ field: column, header: column}));
      console.log("AHAHAHAHAHAHAH");
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


  exportPdf() {
    import('jspdf').then((jsPDF) => {
        import('jspdf-autotable').then((x) => {
            const doc = new jsPDF.default('p', 'px', 'a4');
            (doc as any).autoTable(this.exportColumns, this.tableData);
            doc.save('products.pdf');
        });
    });
}

  /*
  exportPdf(): void {
    import('jspdf').then((jsPDF) => {
      import('jspdf-autotable').then((autoTable) => {
        const doc = new jsPDF.default('p', 'px', 'a4');
        autoTable.default(doc, {
          head: [this.exportColumns.map((column) => column.header)],
          body: this.tableData.map((data) => this.exportColumns.map((column) => data[column.field]))
        });
        doc.save('Data.pdf');
      });
    });
  } */

exportExcel() {
    import('xlsx').then((xlsx) => {
        const worksheet = xlsx.utils.json_to_sheet(this.tableData);
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
