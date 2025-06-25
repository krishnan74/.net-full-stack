import { TestBed } from '@angular/core/testing';
import { RecipeService } from './recipe.service';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { mockRecipe } from './mockdata';

describe('RecipeService', () => {
  let service: RecipeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        RecipeService,
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });
    service = TestBed.inject(RecipeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should retrieve recipe from API', () => {

    service.getRecipeById(1).subscribe((recipe) => {
      expect(recipe).toEqual(mockRecipe);
    });
    const req = httpMock.expectOne('https://dummyjson.com/recipes/1');
    expect(req.request.method).toBe('GET');
    req.flush(mockRecipe);
  });
});