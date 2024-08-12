import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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
   private apiKey = "Produtos 123456789"

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.get<Department[]>(this.apiUrl, { headers});
  }


}
