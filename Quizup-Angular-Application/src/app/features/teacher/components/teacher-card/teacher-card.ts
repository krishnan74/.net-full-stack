import { Component, Input } from '@angular/core';
import { Teacher } from '../../models/teacher';
import { QuizModel } from '../../../quiz/models/quiz.model';

@Component({
  selector: 'app-teacher-card',
  imports: [],
  templateUrl: './teacher-card.html',
  styleUrl: './teacher-card.css',
})
export class TeacherCard {
  @Input() teacher: Teacher | null = new Teacher();
}
