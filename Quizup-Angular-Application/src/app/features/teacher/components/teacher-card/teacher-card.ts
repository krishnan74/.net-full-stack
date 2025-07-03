import { Component, Input } from '@angular/core';
import { TeacherModel } from '../../models/teacher';
import { QuizModel } from '../../../quiz/models/quiz.model';

@Component({
  selector: 'app-teacher-card',
  imports: [],
  templateUrl: './teacher-card.html',
  styleUrl: './teacher-card.css',
})
export class TeacherCard {
  @Input() teacher: TeacherModel | null = new TeacherModel();
}
