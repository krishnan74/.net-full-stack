import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../../services/teacher.service';
import { Teacher } from '../../models/teacher';
import { CommonModule } from '@angular/common';
import { TeacherCard } from '../teacher-card/teacher-card';
import { ApiResponse } from '../../../../shared/models/api-response';

@Component({
  selector: 'app-teacher-list',
  imports: [CommonModule, TeacherCard],
  templateUrl: './teacher-list.html',
  styleUrl: './teacher-list.css',
})
export class TeacherList implements OnInit {
  teachers: Teacher[] = [];

  constructor(private teacherService: TeacherService) {}

  ngOnInit() {
    this.getAllTeachers();
  }

  getAllTeachers() {
    this.teacherService
      .getAllTeachers()
      .subscribe((response: ApiResponse<Teacher[]>) => {
        this.teachers = response.data;
      });
  }
}
