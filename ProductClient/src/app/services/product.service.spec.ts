import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductService, Product } from './product.service';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });


  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });


  it('deve recuperar os produtos da api via get', () => {
    const mockProducts: Product[] = [
      { id: 1, code: '001', description: 'Arroz', price: 10, status: true, departmentId: 1 },
      { id: 2, code: '002', description: 'Pepsi', price: 20, status: false, departmentId: 2 }
    ];

    service.getProducts().subscribe(products => {
      expect(products.length).toBe(2);
      expect(products).toEqual(mockProducts);
    });

    const request = httpMock.expectOne(service['apiUrl']);
    expect(request.request.method).toBe('GET');
    request.flush(mockProducts);
  });


  it('deve recuperar um produto pelo id da api via get', () => {
    const mockProduct: Product = { id: 1, code: '001', description: 'Arroz', price: 10, status: true, departmentId: 1 };

    service.getProductById(1).subscribe(product => {
      expect(product).toEqual(mockProduct);
    });

    const request = httpMock.expectOne(`${service['apiUrl']}/1`);
    expect(request.request.method).toBe('GET');
    request.flush(mockProduct);
  });

  //Faltou tempo para criar o restante dos testes :(


  afterEach(() => {
    httpMock.verify();
  });
});
