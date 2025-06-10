import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { First } from './first/first';
import { Customer } from './customer/customer';
import { Product } from './product/product';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, First, Customer, Product],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'myapp';
}
