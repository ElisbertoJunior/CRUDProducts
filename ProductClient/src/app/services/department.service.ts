import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Department {
  id: number;
  code: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

   private apiUrl = 'http://localhost:5167/api/Department'

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    return this.http.get<Department[]>(this.apiUrl);
  }


}
