import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DepartmentService } from './../../services/department.service';
import { DepartmentListComponent } from './department-list.component';
import { of } from 'rxjs';

describe('DepartmentListComponent', () => {
    let component: DepartmentListComponent;
    let fixture: ComponentFixture<DepartmentListComponent>;
    let departmentService: jasmine.SpyObj<DepartmentService>;

    beforeEach(() => {

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


    it('deve criar', () => {
      expect(component).toBeTruthy();
    });


    it('deve carregar o departamento na inicializaçao', () => {
      const departments = [
          { id: 1, code: '010', description: 'LATICIOS' },
          { id: 2, code: '020', description: 'BEBIDAS' }
      ];

      departmentService.getDepartments.and.returnValue(of(departments));

      component.ngOnInit();


      expect(component.departments).toEqual(departments);

      expect(departmentService.getDepartments).toHaveBeenCalled();
  });

});
