import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [ CommonModule, RouterOutlet],
  templateUrl: './auth.html',
  styleUrls: ['./auth.css'],
})

export class AuthComponent{

}