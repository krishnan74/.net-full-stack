import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../student/services/student.service';
import { TeacherService } from '../../teacher/services/teacher.service';
import { SubjectService } from '../../subject/services/subject.service';
import { ClassService } from '../../class/services/class.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
  imports: [RouterLink, CommonModule],
})
export class AdminDashboardComponent implements OnInit {
  studentCount: number = 0;
  teacherCount: number = 0;
  subjectCount: number = 0;
  classCount: number = 0;

  constructor(
    private studentService: StudentService,
    private teacherService: TeacherService,
    private subjectService: SubjectService,
    private classService: ClassService
  ) {}

  ngOnInit() {
    this.loadStats();
  }

  private loadStats() {
    // Load student count
    this.studentService.getAllStudents().subscribe({
      next: (students) => {
        this.studentCount = students.data.length;
      },
      error: (error) => {
        console.error('Error loading students:', error);
      },
    });

    // Load teacher count
    this.teacherService.getAllTeachers().subscribe({
      next: (teachers) => {
        this.teacherCount = teachers.data.length;
      },
      error: (error) => {
        console.error('Error loading teachers:', error);
      },
    });

    // Load subject count
    this.subjectService.getAllSubjects().subscribe({
      next: (subjects) => {
        this.subjectCount = subjects.data.length;
      },
      error: (error) => {
        console.error('Error loading subjects:', error);
      },
    });

    // Load class count
    this.classService.getAllClasses().subscribe({
      next: (classes) => {
        this.classCount = classes.data.length;
      },
      error: (error) => {
        console.error('Error loading classes:', error);
      },
    });
  }
}
