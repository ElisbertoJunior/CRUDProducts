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



    deleteProduct(productId: number): void {
      const confirmDelete = confirm('Você tem certeza que deseja excluir este produto?');
      if (confirmDelete) {
        // Chama o método para excluir o produto
        this.productService.deleteProduct(productId).subscribe(() => {
          this.loadProducts();
        });
      }
    }

}
