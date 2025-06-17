import { Component, HostListener, OnInit } from '@angular/core';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { Product } from '../product/product';
import { FormsModule } from '@angular/forms';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  tap,
} from 'rxjs';

@Component({
  selector: 'app-products',
  imports: [Product, FormsModule],
  templateUrl: './products.html',
  styleUrl: './products.css',
})
export class Products implements OnInit {
  products: ProductModel[] = [];
  cartCount: number = 0;
  searchString: string = '';
  searchSubject = new Subject<string>();
  loading: boolean = false;
  showBackToTop: boolean = false;
  limit = 10;
  skip = 0;
  total = 0;
  constructor(private productService: ProductService) {}
  handleSearchProducts() {
    this.searchSubject.next(this.searchString);
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => (this.loading = true)),
        switchMap((query) =>
          this.productService.getProductSearchResult(
            query,
            this.limit,
            this.skip
          )
        ),
        tap(() => (this.loading = false))
      )
      .subscribe({
        next: (data: any) => {
          this.products = data.products as ProductModel[];
          this.total = data.total;
          console.log(this.total);
        },
      });
  }
  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - 100;

    if (
      scrollPosition >= threshold &&
      !this.loading &&
      this.products?.length < this.total
    ) {
      console.log('Scroll position: ' + scrollPosition);
      console.log('Threshold: ' + threshold);

      this.loadMore();
    }

    this.showBackToTop = window.scrollY > 300;
  }

  scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  loadMore() {
    if (this.loading || this.products.length >= this.total) {
      return;
    }
    this.loading = true;
    this.skip += this.limit;

    const currentScroll = window.scrollY;
    this.productService
      .getProductSearchResult(this.searchString, this.limit, this.skip)
      .subscribe({
        next: (data: any) => {
          this.products = [...this.products, ...data.products];
          this.loading = false;

          setTimeout(() => {
            window.scrollTo(0, currentScroll);
          }, 0);
        },
        error: () => {
          this.loading = false;
        },
      });
  }
}
