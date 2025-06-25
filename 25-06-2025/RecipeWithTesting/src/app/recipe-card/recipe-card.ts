import { Component, inject, Input } from '@angular/core';
import { RecipeModel } from '../models/recipe';

@Component({
  selector: 'app-recipe-card',
  imports: [],
  templateUrl: './recipe-card.html',
  styleUrl: './recipe-card.css'
})
export class RecipeCard {
@Input() recipe:RecipeModel|null = new RecipeModel();

}