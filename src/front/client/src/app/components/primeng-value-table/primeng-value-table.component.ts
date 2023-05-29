import { Component, Input, OnInit, ViewChild } from '@angular/core';
import * as FileSaver from 'file-saver';
import { Table } from 'primeng/table';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-primeng-value-table',
  templateUrl: './primeng-value-table.component.html',
  styleUrls: ['./primeng-value-table.component.css']
})
export class PrimengValueTableComponent implements OnInit {


  @ViewChild('dt1') dt1: Table | undefined;
  @Input() tableData: any[] = [];
  @Input() columns: any[] = [];
  @Input() columnLabels: any[] = [];
  loading: boolean = true;
  selectedData: any[] = [];
  exportColumns: any[] = [];

  ngOnInit() {
    this.loading = true;
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
/*
exportPdf() {
  this.exportColumns = this.columns.map((col) => ({ title: col.header, dataKey: col.field }));
  console.log(this.exportColumns);
  import('jspdf').then((jsPDF) => {
      import('jspdf-autotable').then((x) => {
          const doc = new jsPDF.default('l', 'px', 'a4');
          (doc as any).autoTable(this.exportColumns, this.tableData);
          doc.save('products.pdf');
      });
  });
}*/
/*
exportExcel() {
    import('xlsx').then((xlsx) => {
        const dataWithHeaders = [];
        const headerRow = this.columns.map(col => col.header);
        dataWithHeaders.push(headerRow);

        // Add data rows
        for (const item of this.tableData) {
          const dataRow = this.columns.map(col => item[col.field]);
          dataWithHeaders.push(dataRow);
        }

        const worksheet = xlsx.utils.aoa_to_sheet(dataWithHeaders);
        const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
        const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
        this.saveAsExcelFile(excelBuffer, 'Data');
    });
}*/

exportExcel() {
  /*
  import('xlsx').then((xlsx) => {
      const worksheet = xlsx.utils.json_to_sheet(this.tableData);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ['data'] };
      const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
      this.saveAsExcelFile(excelBuffer, 'Data');
  }); */
  //let Heading = [['FirstName', 'Last Name', 'Email']];

  //Had to create a new workbook and then add the header
  let Heading = [this.columnLabels];
  const wb = XLSX.utils.book_new();
  const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet([]);
  XLSX.utils.sheet_add_aoa(ws, Heading);

  //Starting in the second row to avoid overriding and skipping headers
  XLSX.utils.sheet_add_json(ws, this.tableData, { origin: 'A2', skipHeader: true });

  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

  XLSX.writeFile(wb, 'Data.xlsx');
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
