import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
// import { Products } from './products/products';
import { Recipes } from './recipes/recipes';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Recipes],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'myapp';
}
