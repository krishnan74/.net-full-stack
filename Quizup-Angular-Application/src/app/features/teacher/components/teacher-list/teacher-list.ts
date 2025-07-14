import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../../services/teacher.service';
import { TeacherModel } from '../../models/teacher.model';
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
  teachers: TeacherModel[] = [];

  constructor(private teacherService: TeacherService) {}

  ngOnInit() {
    this.getAllTeachers();
  }

  getAllTeachers() {
    this.teacherService
      .getAllTeachers()
      .subscribe((response: ApiResponse<TeacherModel[]>) => {
        this.teachers = response.data;
      });
  }
}
