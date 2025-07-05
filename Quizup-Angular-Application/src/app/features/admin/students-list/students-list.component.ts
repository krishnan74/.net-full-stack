import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../student/services/student.service';
import { StudentModel } from '../../student/models/student';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  imports: [CommonModule],
  styleUrls: ['./students-list.component.css'],
})
export class StudentsListComponent implements OnInit {
  students: StudentModel[] = [];

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getAllStudents().subscribe((data) => {
      this.students = data.data;
      console.log('Students loaded:', this.students);
    });
  }

  updateStudent(student: any): void {
    // Logic to update student
  }
}
