import { Component, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService, Product } from 'src/app/services/product.service';


@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss']
})
export class ProductFormComponent implements OnInit {

    product: Product = {
      id: 0,
      code: '',
      description: '',
      price: 0,
      status: true,
      departmentId: 0
    }

    isEditMode: boolean = false;

    constructor(
      private route: ActivatedRoute,
      private router: Router,
      private productService: ProductService
    ){}

    ngOnInit(): void {
      const id = this.route.snapshot.params['id'];
      if(id) {
          this.isEditMode = true;
          this.productService.getProductById(id).subscribe(product => {
            this.product = product;
          });
      }
    }

    saveProduct(): void {
      if(this.isEditMode) {
        this.productService.updateProduct(this.product.id, this.product).subscribe(() => {
          this.router.navigate(['/products']);
        });
      } else {
        this.productService.createProduct(this.product).subscribe(() => {
          this.router.navigate(['/products']);
        })
      }
    }

}
