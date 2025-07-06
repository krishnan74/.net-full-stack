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

@Component({
  selector: 'app-quiz-create',
  templateUrl: './quiz-create.component.html',
  styleUrls: ['./quiz-create.component.css'],
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
})
export class QuizCreateComponent implements OnInit {
  quizForm: FormGroup;
  availableSubjects: any[] = [];
  availableClasses: any[] = [];
  availableTeachers: any[] = [];
  availableQuestions: any[] = [];
  filteredQuestions: any[] = [];
  selectedQuestions: any[] = [];
  questionSearch: string = '';

  constructor(private fb: FormBuilder) {
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      dueDate: ['', Validators.required],
      teacherId: ['', Validators.required],
      subjectId: ['', Validators.required],
      classId: ['', Validators.required],
      questions: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    // TODO: Load availableSubjects, availableClasses, availableTeachers, availableQuestions from services
    this.filteredQuestions = this.availableQuestions;
  }

  get questionsArray(): FormArray {
    return this.quizForm.get('questions') as FormArray;
  }

  addQuestion(question: any) {
    if (!this.selectedQuestions.find((q) => q.text === question.text)) {
      this.selectedQuestions.push(question);
      this.questionsArray.push(
        this.fb.group({
          text: [question.text, Validators.required],
          options: [question.options, Validators.required],
          correctAnswer: [question.correctAnswer, Validators.required],
        })
      );
    }
  }

  removeQuestion(index: number) {
    this.selectedQuestions.splice(index, 1);
    this.questionsArray.removeAt(index);
  }

  searchQuestions() {
    const term = this.questionSearch.toLowerCase();
    this.filteredQuestions = this.availableQuestions.filter((q) =>
      q.text.toLowerCase().includes(term)
    );
  }

  onSubmit() {
    if (this.quizForm.valid) {
      // Final data to send
      const quizData = {
        ...this.quizForm.value,
        questions: this.selectedQuestions,
      };
      console.log('Quiz Data:', quizData);
      // TODO: Call quiz creation service
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

  onDragStart(event: DragEvent, question: any) {
    if (event.dataTransfer) {
      event.dataTransfer.setData('question', JSON.stringify(question));
    }
  }
}
