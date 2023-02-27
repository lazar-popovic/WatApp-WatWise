import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Student {
  id: number;
  name: string;
  number: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  students!: Student[];

  constructor(private http:HttpClient){}

  getStudents(): void {
    this.http.get<Student[]>('https://localhost:44373/api/Values')
      .subscribe(students => this.students = students);
    }

    createStudent(student: Student): void {
      this.http.post<Student>('https://localhost:44371/api/Values', student)
        .subscribe(() => {
          this.getStudents();
          });
    }
    deleteStudent(student: Student): void {
      this.http.delete(`https://localhost:44371/api/Values/${student.id}`)
        .subscribe(() => {
          this.getStudents();
        });


      }
  

}


