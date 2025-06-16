import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from './products/products';
import { Home } from './home/home';
import { Navbar } from './navbar/navbar';
import { About } from './about/about';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet , Products, Navbar, About, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'app';
}
