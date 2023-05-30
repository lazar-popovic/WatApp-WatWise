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

exportExcel() {
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

public exportToCsv() {
  let Heading = [this.columnLabels];
  const ws: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet(Heading);

  //Starting in the second row to avoid overriding and skipping headers
  XLSX.utils.sheet_add_json(ws, this.tableData, { origin: 'A2', skipHeader: true });

  // Convert the worksheet to a CSV string
  const csvData = XLSX.utils.sheet_to_csv(ws);
  this.saveAsFile(csvData, 'Data.csv', 'text/csv');
}
private saveAsFile(buffer: any, fileName: string, fileType: string): void {
  const data: Blob = new Blob([buffer], { type: fileType });
  FileSaver.saveAs(data, fileName);
}

}
