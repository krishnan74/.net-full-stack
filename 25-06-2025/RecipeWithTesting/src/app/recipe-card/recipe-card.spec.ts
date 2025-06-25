import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RecipeCard } from './recipe-card';
import { RecipeModel } from '../models/recipe';
import { Component } from '@angular/core';

describe('RecipeCard', () => {
  let component: RecipeCard;
  let fixture: ComponentFixture<RecipeCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecipeCard]
    }).compileComponents();

    fixture = TestBed.createComponent(RecipeCard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display recipe name, cuisine, cook time, and ingredients', () => {
    const testRecipe: RecipeModel = {
      id: 1,
      name: 'Classic Margherita Pizza',
      cuisine: 'Italian',
      cookTimeMinutes: 15,
      image: 'https://cdn.dummyjson.com/recipe-images/1.webp',
      ingredients: ['Pizza dough', 'Tomato Sauce', 'Fresh mozzarella cheese', 'Fresh basil leaves', 'Olive oil' ,'Salt and pepper to taste']
    };
    component.recipe = testRecipe;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.card-title')?.textContent).toContain('Classic Margherita Pizza');
    expect(compiled.querySelector('.cuisine')?.textContent).toContain('Italian');
    expect(compiled.querySelector('.cookTime')?.textContent).toContain('15 minutes');
    expect(compiled.querySelector('.ingredient:nth-child(1)')?.textContent).toContain('Pizza dough');
    expect(compiled.querySelector('.ingredient:nth-child(2)')?.textContent).toContain('Tomato Sauce');
    expect(compiled.querySelector('.ingredient:nth-child(3)')?.textContent).toContain('Fresh mozzarella cheese');
    expect(compiled.querySelector('.ingredient:nth-child(4)')?.textContent).toContain('Fresh basil leaves');
    expect(compiled.querySelector('.ingredient:nth-child(5)')?.textContent).toContain('Olive oil');
    expect(compiled.querySelector('.ingredient:nth-child(6)')?.textContent).toContain('Salt and pepper to taste');
  });
});
