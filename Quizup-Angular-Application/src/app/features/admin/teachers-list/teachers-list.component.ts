import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../../teacher/services/teacher.service';
import { TeacherModel } from '../../teacher/models/teacher';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-teachers-list',
  templateUrl: './teachers-list.component.html',
  imports: [CommonModule],
  styleUrls: ['./teachers-list.component.css'],
})
export class TeachersListComponent implements OnInit {
  teachers: TeacherModel[] = [];

  constructor(private teacherService: TeacherService) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers(): void {
    this.teacherService.getAllTeachers().subscribe((data) => {
      this.teachers = data.data;
      console.log('Teachers loaded:', this.teachers);
    });
  }

  updateTeacher(teacher: any): void {
    // Logic to update teacher
  }
}
