import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormArray,
  ReactiveFormsModule,
  FormsModule,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { User } from '../../../../store/auth/auth.model';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { ProfileService } from '../../../profile/profile.service';
import { Observable, switchMap, of } from 'rxjs';
import { ClassModel } from '../../../class/models/class';
import { SubjectModel } from '../../../subject/models/subject';
import { QuestionModel } from '../../models/question.model';

@Component({
  selector: 'app-quiz-create',
  templateUrl: './quiz-create.component.html',
  styleUrls: ['./quiz-create.component.css'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
})
export class QuizCreateComponent implements OnInit {
  user$ = this.store.select(selectUser);
  user: User | null = null;

  quizForm: FormGroup;
  availableQuestions: QuestionModel[] = [];
  filteredQuestions: QuestionModel[] = [];
  selectedQuestions: QuestionModel[] = [];
  questionSearch: string = '';
  newQuestionOptions: string[] = ['', '', '', '']; 
  newQuestionCorrectAnswer: string | null = null; 

  constructor(private fb: FormBuilder, private profileService: ProfileService, private store: Store) {

    this.user$.subscribe((user) => {
      this.user = user;
    });
      
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      subjectId: ['', Validators.required],
      classId: ['', Validators.required],
    });

  }

  ngOnInit(): void {
    // Fetch available questions for the teacher
    this.questions.subscribe((questions) => {
      if (questions) {
        this.availableQuestions = questions;
        this.filteredQuestions = questions; // Initialize filteredQuestions
      }
    });
  }

  teacherSubjects$: Observable<SubjectModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.profileService.getSubjectsByTeacherId(user.userId)
        : of(null)
    )
  );

  teacherClasses$: Observable<ClassModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.profileService.getClassesByTeacherId(user.userId)
        : of(null)
    )
  );

  questions: Observable<QuestionModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.profileService.getQuestionsByTeacherId(user.userId)
        : of(null)
    )
  );

  addQuestion(question: QuestionModel) {
    if (!this.selectedQuestions.find((q) => q.id === question.id)) {
      this.selectedQuestions.push(question);
      
    }
  }

  removeQuestion(index: number) {
    this.selectedQuestions.splice(index, 1);
  }

  searchQuestions() {
    const term = this.questionSearch.toLowerCase();
    this.filteredQuestions = this.availableQuestions.filter((q) =>
      q.text.toLowerCase().includes(term)
    );
  }

  createNewQuestion() {

    console.log('Creating new question:', this.questionSearch, this.newQuestionOptions, this.newQuestionCorrectAnswer);
    const newQuestion: QuestionModel = {
      id: null, 
      text: this.questionSearch.trim(),
      options: [...this.newQuestionOptions], 
      correctAnswer: this.newQuestionCorrectAnswer, 
    };

    this.addQuestion(newQuestion);

    this.questionSearch = '';
    this.newQuestionOptions = ['', '', '', '']; // Reset to four empty options
    this.newQuestionCorrectAnswer = null;
  }

  onSubmit() {
    console.log('Form Submitted:', this.quizForm.value);
    if (this.quizForm.valid) {
      const quizData = {
        ...this.quizForm.value,
        teacherId: this.user?.userId,
        questions: this.selectedQuestions,
      };
      console.log('Quiz Data:', quizData);
    }
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    const data = event.dataTransfer?.getData('question');
    if (data) {
      const question = JSON.parse(data);
      this.addQuestion(question);
    }
  }

  onDragStart(event: DragEvent, question: QuestionModel) {
    if (event.dataTransfer) {
      event.dataTransfer.setData('question', JSON.stringify(question));
    }
  }

  isCreateButtonDisabled(): boolean {
    console.log('Checking if create button is disabled');
    console.log('New Question Options:', this.newQuestionOptions);
    return this.newQuestionOptions.some((opt) => opt.trim() === '') || this.questionSearch.trim() === '';
  }
}
