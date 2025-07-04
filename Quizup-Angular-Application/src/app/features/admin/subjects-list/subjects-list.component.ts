import { Component, OnInit } from '@angular/core';
import { SubjectService } from '../../subject/services/subject.service';
import { CommonModule } from '@angular/common';
import { SubjectModel } from '../../subject/models/subject';
@Component({
  selector: 'app-subjects-list',
  templateUrl: './subjects-list.component.html',
  imports: [CommonModule],
  styleUrls: ['./subjects-list.component.css'],
})
export class SubjectsListComponent implements OnInit {
  subjects: SubjectModel[] = [];

  constructor(private subjectService: SubjectService) {}

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.subjectService.getAllSubjects().subscribe((data) => {
      this.subjects = data.data;
    });
  }

  addOrUpdateSubject(subject: any): void {
    // Logic to add or update subject
  }
}
