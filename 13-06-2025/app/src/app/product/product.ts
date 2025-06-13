import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ProductModel } from '../models/product';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
@Input() product:ProductModel|null = new ProductModel();
@Output() addToCart:EventEmitter<number> = new EventEmitter<number>();

handleBuyClick(pid:number|undefined){
  if(pid)
  {
      this.addToCart.emit(pid);
  }
}
constructor(){

}

}
