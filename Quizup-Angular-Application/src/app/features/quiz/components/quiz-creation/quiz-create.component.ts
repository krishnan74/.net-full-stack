import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormArray,
  ReactiveFormsModule,
  FormsModule,
  AbstractControl,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { User } from '../../../../store/auth/auth.model';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { ProfileService } from '../../../profile/profile.service';
import { Observable, switchMap, of } from 'rxjs';
import { ClassModel } from '../../../class/models/class.model';
import { SubjectModel } from '../../../subject/models/subject.model';
import { QuestionModel } from '../../models/question.model';
import { QuizService } from '../../services/quiz.service';
import { QuizCreateModel } from '../../models/quiz.model';
import { QuestionCard } from '../question-card/question-card';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-quiz-create',
  templateUrl: './quiz-create.component.html',
  styleUrls: ['./quiz-create.component.css'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule, QuestionCard],
})
export class QuizCreateComponent implements OnInit {
  minDueDate: string = '';
  user$ = this.store.select(selectUser);
  user: User | null = null;

  quizForm: FormGroup;
  availableQuestions: QuestionModel[] = [];
  filteredQuestions: QuestionModel[] = [];
  selectedQuestions: QuestionModel[] = [];
  questionSearch: string = '';
  newQuestionOptions: string[] = ['', '', '', ''];

  newQuestionCorrectAnswer: string | null = null;

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private store: Store,
    private quizService: QuizService,
    private toastService: ToastService
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
    });

    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', [Validators.required, this.futureDateTimeValidator]],
      subjectId: [0, Validators.required],
      classId: [0, Validators.required],
    });
  }

  // Validator to ensure due date is in the future (date and time)
  futureDateTimeValidator(control: AbstractControl) {
    if (!control.value) return null;
    const selected = new Date(control.value);
    const now = new Date();
    // Block any date/time in the past (including today with past time)
    if (selected.getTime() <= now.getTime()) {
      return { pastDateTime: true };
    }
    return null;
  }

  ngOnInit(): void {
    // Set minDueDate to now in local datetime-local format
    const now = new Date();
    const pad = (n: number) => n.toString().padStart(2, '0');
    this.minDueDate = `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(
      now.getDate()
    )}T${pad(now.getHours())}:${pad(now.getMinutes())}`;
    this.questions.subscribe((questions) => {
      if (questions) {
        this.availableQuestions = questions;
        this.filteredQuestions = questions;
      }
    });

    // Ensure subjectId and classId are always numbers in the form
    this.quizForm.get('subjectId')?.valueChanges.subscribe((val) => {
      if (typeof val === 'string') {
        this.quizForm
          .get('subjectId')
          ?.setValue(Number(val), { emitEvent: false });
      }
    });
    this.quizForm.get('classId')?.valueChanges.subscribe((val) => {
      if (typeof val === 'string') {
        this.quizForm
          .get('classId')
          ?.setValue(Number(val), { emitEvent: false });
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
      this.toastService.showInfo(
        'Question Added',
        'Question added to the quiz'
      );
    } else {
      this.toastService.showWarning(
        'Question Already Added',
        'This question is already in the quiz'
      );
    }
  }

  removeQuestion(index: number) {
    this.selectedQuestions.splice(index, 1);
    this.toastService.showInfo(
      'Question Removed',
      'Question removed from the quiz'
    );
  }

  searchQuestions() {
    const term = this.questionSearch.toLowerCase();
    this.filteredQuestions = this.availableQuestions.filter((q) =>
      q.text.toLowerCase().includes(term)
    );
  }

  createNewQuestion() {
    console.log(
      'Creating new question:',
      this.questionSearch,
      this.newQuestionOptions,
      this.newQuestionCorrectAnswer
    );
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
    this.toastService.showSuccess(
      'Question Created',
      'New question created and added to quiz'
    );
  }

  onSubmit() {
    console.log('Form Submitted:', this.quizForm.value);
    if (this.quizForm.valid) {
      if (this.selectedQuestions.length === 0) {
        this.toastService.showWarning(
          'No Questions',
          'Please add at least one question to the quiz'
        );
        return;
      }

      const quizData: QuizCreateModel = {
        title: this.quizForm.value.title,
        description: this.quizForm.value.description,
        dueDate: this.changeLocalDateToUTC(this.quizForm.value.dueDate),
        subjectId: Number(this.quizForm.value.subjectId),
        classId: Number(this.quizForm.value.classId),
        teacherId: this.user?.userId!,
        questions: this.selectedQuestions,
      };
      this.quizService.createQuiz(quizData).subscribe({
        next: (response) => {
          console.log('Quiz created successfully:', response.data);
          this.toastService.showSuccess(
            'Quiz Created',
            `Quiz "${quizData.title}" created successfully!`
          );
          this.quizForm.reset();
          this.selectedQuestions = [];
          this.questionSearch = '';
          this.newQuestionOptions = ['', '', '', ''];
          this.newQuestionCorrectAnswer = null;
        },
        error: (error) => {
          console.error('Error creating quiz:', error);
          this.toastService.showError('Quiz Creation', error);
        },
      });
      console.log('Quiz Data:', quizData);
    } else {
      this.toastService.showValidationError(
        'Quiz Form',
        'Please fill in all required fields correctly.'
      );
      this.quizForm.markAllAsTouched();
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
    console.log('Correct Answer:', this.newQuestionCorrectAnswer);
    return (
      this.newQuestionOptions.some((opt) => opt.trim() === '') ||
      this.questionSearch.trim() === ''
    );
  }

  changeLocalDateToUTC(date: any): string {
    const localDate = new Date(date);
    return localDate.toISOString();
  }
}
