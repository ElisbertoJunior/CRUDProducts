import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductListComponent } from './product-list.component';
import { ProductService } from './../../services/product.service';
import { of } from 'rxjs';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let productService: jasmine.SpyObj<ProductService>;

  beforeEach(() => {
    // Cria um spy do ProductService com os metodos getProducts e deleteProduct
    const productServiceSpy = jasmine.createSpyObj('ProductService', ['getProducts', 'deleteProduct']);

    TestBed.configureTestingModule({
      declarations: [ProductListComponent],
      providers: [
        { provide: ProductService, useValue: productServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService) as jasmine.SpyObj<ProductService>;
  });


  it('deve criar', () => {
    expect(component).toBeTruthy();
  });

  it('deve carregar os produtos na inicializaçao', () => {
    const products = [
      { id: 1, code: '001', description: 'Arroz', price: 10, status: true, department: { id: 1, code: '010', description: 'CEREAIS' } },
      { id: 2, code: '002', description: 'Pepsi', price: 20, status: false, department: { id: 2, code: '020', description: 'BEBIDAS' } }
    ];
    productService.getProducts.and.returnValue(of(products));

    component.ngOnInit();

    expect(component.products).toEqual(products);
    expect(productService.getProducts).toHaveBeenCalled();
  });


  it('deve excluir o produto e recarregar os produtos', () => {
    const products = [
      { id: 1, code: '001', description: 'Leite', price: 10, status: true, department: { id: 1, code: '010', description: 'LATICINIOS' } },
      { id: 2, code: '002', description: 'Chocolate', price: 20, status: false, department: { id: 2, code: '020', description: 'MERCEARIA' } }
    ];

    productService.getProducts.and.returnValue(of(products));
    productService.deleteProduct.and.returnValue(of(undefined));

    component.ngOnInit();
    component.deleteProduct(1);

    expect(productService.deleteProduct).toHaveBeenCalledWith(1);
    expect(productService.getProducts).toHaveBeenCalledTimes(2);
  });

});
