import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Home } from './home/home';
import { Login } from './login/login';
import { About } from './about/about';
import { Contact } from './contact/contact';
import { Navbar } from './navbar/navbar';
import { Products } from './products/products';
import { ProductDetail } from './product-detail/product-detail';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'app';
}
