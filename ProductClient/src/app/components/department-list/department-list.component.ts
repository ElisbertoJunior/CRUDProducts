import { Component, OnInit } from '@angular/core';
import { DepartmentService, Department } from 'src/app/services/department.service';

@Component({
  selector: 'app-department-list',
  templateUrl: './department-list.component.html',
})
export class DepartmentListComponent implements OnInit {
  departments: Department[] = [];

  constructor(private departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.loadDepartment()
  }

  loadDepartment(): void {
    console.log('Carregando departamentos...');
    this.departmentService.getDepartments().subscribe(departments => {
    console.log(departments);
    this.departments = departments;
  }, error => {
    console.error('Erro ao carregar departamentos:', error);
  });
  }
}

