import { Component, OnInit, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit {
  recipes = signal<RecipeModel[] | undefined>(undefined);

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe({
      next: (data: any) => {
        this.recipes.set(data.recipes as RecipeModel[]);
      },
      error: (err) => {},
      complete: () => {}
    });
  }
}