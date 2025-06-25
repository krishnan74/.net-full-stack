import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe';
import { Component } from '@angular/core';
import { of } from 'rxjs';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

class MockRecipeService {
  getAllRecipes() {
    return of({ recipes: [
      {
      id: 1,
      name: 'Classic Margherita Pizza',
      cuisine: 'Italian',
      cookTimeMinutes: 15,
      image: 'https://cdn.dummyjson.com/recipe-images/1.webp',
      ingredients: ['Pizza dough', 'Tomato Sauce', 'Fresh mozzarella cheese', 'Fresh basil leaves', 'Olive oil' ,'Salt and pepper to taste']
    }, {
      id: 2,
      name: 'Vegetarian Stir-Fry',
      cuisine: 'Asian',
      cookTimeMinutes: 20,
      image: 'https://cdn.dummyjson.com/recipe-images/2.webp',
      ingredients: ['Tofu, cubed', 'Broccoli florets', 'Carrots, sliced', 'Bell peppers, sliced', 'Soy sauce', 'Ginger, minced', 'Garlic, minced', 'Sesame oil', 'Cooked rice for serving']
    }
    ] });
  }
}

@Component({
  standalone: true,
  imports: [Recipes],
  template: `<app-recipes></app-recipes>`
})
class HostComponent {
  recipes: RecipeModel[] = [];
}

describe('Recipes', () => {
  let fixture: ComponentFixture<HostComponent>;
  let hostComponent: HostComponent;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostComponent],
      providers: [
        { provide: RecipeService, useClass: MockRecipeService },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HostComponent);
    hostComponent = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(hostComponent).toBeTruthy();
  });

  it('should render recipe list when recipes are available', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('List of Recipes you might enjoy');
    expect(compiled.textContent).toContain('Classic Margherita Pizza');
    expect(compiled.textContent).toContain('Italian');
  });

  it('should render "No recipes available" when recipes is empty', () => {
    const recipeService = TestBed.inject(RecipeService);
    spyOn(recipeService, 'getAllRecipes').and.returnValue(of([]));

    fixture = TestBed.createComponent(HostComponent);
    hostComponent = fixture.componentInstance;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No recipes available');
  });
});
