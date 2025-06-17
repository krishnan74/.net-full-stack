import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ProductService } from '../services/product.service';
import { ProductModel } from '../models/product';
import { ProductDetailModel } from '../models/product-detail';

@Component({
  selector: 'app-product-detail',
  imports: [RouterLink],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.css'
})
export class ProductDetail {
  router = inject(ActivatedRoute);
  loading: boolean = true;
  notFound: boolean = false;

  productId: string = "";
  ratingArray: number[] =[];

  product:ProductDetailModel = new ProductDetailModel();

  constructor(private productService: ProductService) {}



  ngOnInit(): void {
    console.log("init");
    this.productId = this.router.snapshot.params["id"] as string
    
    this.productService.getProduct(
      Number(this.productId)
    ).subscribe({
      next: (data: any) => {
          this.product = data as ProductDetailModel;
          this.ratingArray = new Array(data.rating);
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.notFound = true;
          console.error("Product not found");
        },
    }
    )
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-IN', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }
  
}
