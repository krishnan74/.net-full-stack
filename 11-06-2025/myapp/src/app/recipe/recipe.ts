import { Component, inject, Input } from '@angular/core';
import { RecipeModel } from '../models/recipe';

@Component({
  selector: 'app-recipe',
  imports: [],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {
@Input() recipe:RecipeModel|null = new RecipeModel();

}