import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css']
})
export class CrudComponent 
{
  items: any[] = [];
  newItem: any = {};
  editItems: any = {};
  editMode = false;

  constructor(private http: HttpClient) { }

  getAllItems() {
    this.http.get<any[]>('/api/items').subscribe(items => {
      this.items = items;
    });
  }

  createItem() {
    this.http.post<any>('/api/items', this.newItem).subscribe(item => {
      this.items.push(item);
      this.newItem = {};
    });
  }

  editItem(item: any) {
    this.editItem = { ...item };
    this.editMode = true;
  }

  updateItem() {
    this.http.put<any>(`/api/items/${this.editItems.id}`, this.editItem).subscribe(item => {
      const index = this.items.findIndex(i => i.id === item.id);
      this.items[index] = item;
      this.editItems = {};
      this.editMode = false;
    });
  }

  deleteItem(item: any) {
    this.http.delete(`/api/items/${item.id}`).subscribe(() => {
      const index = this.items.findIndex(i => i.id === item.id);
      this.items.splice(index, 1);
    });
  }

  cancelEdit() {
    this.editItems = {};
    this.editMode = false;
  }

  ngOnInit() {
    this.getAllItems();
  }
}
