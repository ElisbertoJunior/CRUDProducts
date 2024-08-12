import { Component, OnInit} from '@angular/core';
import { Department, DepartmentService } from './../../services/department.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService, Product } from 'src/app/services/product.service';


@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
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

    departments: Department[] = []

    constructor(
      private route: ActivatedRoute,
      private router: Router,
      private productService: ProductService,
      private departmentService: DepartmentService
    ){}

    ngOnInit(): void {
      this.loadDepartments();
      const id = this.route.snapshot.params['id'];
      if(id) {
          this.isEditMode = true;
          this.productService.getProductById(id).subscribe(product => {
            this.product = product;

            console.log('Loaded product:', this.product);
          });
      }
    }

    loadDepartments(): void {
      this.departmentService.getDepartments().subscribe(departments => {
          this.departments = departments;
      });
    }

    saveProduct(): void {
      const updatedProduct = {
        id: this.product.id,
        code: this.product.code,
        description: this.product.description,
        price: this.product.price,
        status: this.product.status,
        departmentId: this.product.departmentId || this.product.department?.id
      };

      if(this.isEditMode) {
        this.productService.updateProduct(this.product.id, updatedProduct).subscribe(response => {
          this.router.navigate(['/products']);
        },  error => alert(error));
      } else {
        this.productService.createProduct(this.product).subscribe(() => {
          this.router.navigate(['/products']);
        }, error => alert(error));
      }
    }

}
