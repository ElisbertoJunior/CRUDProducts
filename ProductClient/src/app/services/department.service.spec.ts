import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DepartmentService, Department } from './department.service';

describe('DepartmentService', () => {
  let service: DepartmentService;
  let httpMock: HttpTestingController;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DepartmentService]
    });

    service = TestBed.inject(DepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  // Testa se o serviço foi criado corretamente
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // Teste para verificar se getDepartments retorna uma lista de departamentos
  it('should retrieve departments from the API via GET', () => {
    const mockDepartments: Department[] = [
      { id: 1, code: '010', description: 'BEBIDAS' },
      { id: 2, code: '020', description: 'CONGELADOS' }
    ];

    service.getDepartments().subscribe(departments => {
      expect(departments.length).toBe(2);
      expect(departments).toEqual(mockDepartments);
    });

    // verifica se uma requisiçao HTTP foi feita
    const request = httpMock.expectOne(service['apiUrl']);
    expect(request.request.method).toBe('GET');
    request.flush(mockDepartments);
  });

  // verifica se nao ha requisiçoes pendentes apos cada teste
  afterEach(() => {
    httpMock.verify();
  });
});
