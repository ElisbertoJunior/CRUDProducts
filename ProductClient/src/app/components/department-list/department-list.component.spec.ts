import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DepartmentService } from './../../services/department.service';
import { DepartmentListComponent } from './department-list.component';
import { of } from 'rxjs';

describe('DepartmentListComponent', () => {
    let component: DepartmentListComponent;
    let fixture: ComponentFixture<DepartmentListComponent>;
    let departmentService: jasmine.SpyObj<DepartmentService>;

    beforeEach(() => {
       // Cria um objeto espiao para o DepartmentService que simula a funçao getDepartments
      const departmentServiceSpy = jasmine.createSpyObj('DepartmentService', ['getDepartments']);

      TestBed.configureTestingModule({
        declarations: [DepartmentListComponent],
        providers: [
          { provide: DepartmentService, useValue: departmentServiceSpy }
        ]
      }).compileComponents();

      fixture = TestBed.createComponent(DepartmentListComponent);
      component = fixture.componentInstance;
      departmentService = TestBed.inject(DepartmentService) as jasmine.SpyObj<DepartmentService>
    });

    // Teste basico para verificar se o componente e criado corretamente
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    // Teste para verificar se os departamentos sao carregados corretamente no metodo ngOnInit
    it('should load department on init', () => {
      const departments = [
          { id: 1, code: '010', description: 'LATICIOS' },
          { id: 2, code: '020', description: 'BEBIDAS' }
      ];
      // Simula o retorno da funçao getDepartments para retornar o array de departamentos
      departmentService.getDepartments.and.returnValue(of(departments));

      component.ngOnInit();

      // Verifica se a lista de departamentos do componente e igual a que foi retornada pelo serviço
      expect(component.departments).toEqual(departments);
      // Verifica se o metodo getDepartments foi chamado
      expect(departmentService.getDepartments).toHaveBeenCalled();
  });

});
