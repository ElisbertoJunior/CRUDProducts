import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from 'src/app/services/product.service';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
    products: Product[] = [];

    constructor(private productService: ProductService) {}

    ngOnInit(): void {
      this.loadProducts();
    }

    loadProducts(): void {
      this.productService.getProducts().subscribe(products => {
          this.products = products;
      });
    }

    deleteProduct(id: number): void {
      this.productService.deleteProduct(id).subscribe(() => {
        // Recarrega a lista após a exclusão
        this.loadProducts()
      });
    }
}
