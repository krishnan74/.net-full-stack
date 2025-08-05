import { Component, OnInit } from '@angular/core';
import { QuizService } from '../../services/quiz.service';
import { debounceTime, Subject, switchMap, tap } from 'rxjs';
import { QuizModel } from '../../models/quiz.model';
import { QuizCard } from '../quiz-card/quiz-card';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { User } from '../../../../store/auth/auth.model';
import { ClassService } from '../../../class/services/class.service';
import { SubjectService } from '../../../subject/services/subject.service';
import { ProfileService } from '../../../profile/profile.service';
import { ClassModel } from '../../../class/models/class.model';
import { SubjectModel } from '../../../subject/models/subject.model';
import { ToastService } from '../../../../core/services/toast.service';

@Component({
  selector: 'app-quiz-list',
  imports: [QuizCard, CommonModule, FormsModule],
  templateUrl: './quiz-list.html',
  styleUrl: './quiz-list.css',
})
export class QuizList implements OnInit {
  quizzes: QuizModel[] = [];
  user$ = this.store.select(selectUser);
  user: User | null = null;

  searchString: string = '';
  createdAtMin?: Date;
  createdAtMax?: Date;
  dueDateMin?: Date;
  dueDateMax?: Date;
  isActive?: boolean;
  subjectId?: number;
  classId?: number;

  searchSubject = new Subject<void>();
  loading: boolean = false;
  showFilters: boolean = true;
  filterToggle: boolean = false;
  lastScrollTop: number = 0;

  subjects: SubjectModel[] = [];
  classes: ClassModel[] = [];

  constructor(
    private quizService: QuizService,
    private store: Store,
    private profileService: ProfileService,
    private toastService: ToastService
  ) {
    window.addEventListener('scroll', this.onScroll.bind(this));
    this.user$.subscribe({
      next: (user) => {
        this.user = user;
        if (user) {
          this.loadUserData();
        }
      },
      error: (error) => {
        console.error('Error loading user:', error);
        this.toastService.showError('Loading User', error);
      },
    });
  }

  loadUserData() {
    if (this.user?.role == 'Teacher') {
      this.profileService.getSubjectsByTeacherId(this.user.userId).subscribe({
        next: (subjects) => {
          this.subjects = subjects;
        },
        error: (error) => {
          console.error('Error loading teacher subjects:', error);
          this.toastService.showError('Loading Subjects', error);
        },
      });

      this.profileService.getClassesByTeacherId(this.user.userId).subscribe({
        next: (classes) => {
          this.classes = classes;
        },
        error: (error) => {
          console.error('Error loading teacher classes:', error);
          this.toastService.showError('Loading Classes', error);
        },
      });
    } else if (this.user?.role == 'Student') {
      this.profileService.getSubjectsByStudentId(this.user.userId).subscribe({
        next: (subjects) => {
          this.subjects = subjects;
        },
        error: (error) => {
          console.error('Error loading student subjects:', error);
          this.toastService.showError('Loading Subjects', error);
        },
      });
    }
  }

  handleSearchQuizzes() {
    this.searchSubject.next();
  }

  onScroll() {
    const st = window.scrollY || document.documentElement.scrollTop;
    if (st > this.lastScrollTop && st > 100 && !this.filterToggle) {
      this.showFilters = false;
    } else if (st <= 0) {
      this.showFilters = true;
      this.filterToggle = false;
    }
    this.lastScrollTop = st <= 0 ? 0 : st;
  }

  toggleFilters() {
    this.filterToggle = !this.filterToggle;
    this.showFilters = !this.showFilters;
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.quizService.searchQuizzes(
            this.searchString,
            this.createdAtMin,
            this.createdAtMax,
            this.dueDateMin,
            this.dueDateMax,
            this.isActive,
            this.subjectId,
            this.classId,
            this.user?.role,
            this.user?.userId
          )
        ),
        tap(() => (this.loading = false))
      )
      .subscribe({
        next: (data: any) => {
          console.log('Quizzes fetched:', data.data);
          this.quizzes = data.data as QuizModel[];
          this.toastService.showInfo(
            'Quizzes Loaded',
            `Found ${this.quizzes.length} quizzes`
          );
        },
        error: (error) => {
          console.error('Error fetching quizzes:', error);
          this.toastService.showError('Loading Quizzes', error);
          this.loading = false;
        },
      });

    // Trigger initial search to list all quizzes on page load
    this.handleSearchQuizzes();
  }
}
