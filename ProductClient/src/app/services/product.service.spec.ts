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

  // Teste se o serviço foi criado corretamente
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // Teste para verificar se getProducts retorna uma lista de produtos
  it('should retrieve products from the API via GET', () => {
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

  // Teste para verificar se getProductById retorna um produto especifico
  it('should retrieve a product by ID from the API via GET', () => {
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
