import { Component } from '@angular/core';
import { productData } from "./data.json";
import { CommonModule } from '@angular/common';

type ProductType = {
  id: number;
  name: string;
  price: number;
  description?: string;
  rating?: number;
  imageURL?: string;
  category?: string;
}

@Component({
  selector: 'app-product',
  imports: [CommonModule],
  templateUrl: './product.html',
  styleUrl: './product.css'
})


export class Product {
  products: ProductType[];
  cart: ProductType[] = [];
  cartCount: number = 0;

  constructor() {
    this.products = productData;
  }

  addToCart(product: ProductType) {
    this.cart.push(product);
    this.cartCount = this.cart.length;
    alert(`Product added to cart: ${product.name}`);
  }
}
