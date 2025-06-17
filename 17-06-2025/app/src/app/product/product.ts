import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ProductModel } from '../models/product';
import { Router } from '@angular/router';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css',
})
export class Product {
  @Input() product: ProductModel | null = new ProductModel();
  @Input() searchTerm: string = '';

  constructor(private router: Router) {}


  handleClick(productId: number | undefined): void {
    console.log('Product clicked:', productId);
    if(productId){
      this.router.navigateByUrl(`/products/${productId}`);

    }
  }

  shortenTitle(title: string | undefined): string {
    if (title && title.length > 30) {
      return title.substring(0, 30) + '...';
    }
    return title || '';
  }

  
  

  highlightSearchTerm(text: string): string {
    if (!this.searchTerm) return text;
    const regex = new RegExp(`(${this.searchTerm})`, 'gi');
    return text.replace(regex, '<span class="highlight">$1</span>');
  }

}
