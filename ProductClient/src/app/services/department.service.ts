import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environments';

export interface Department {
  id: number;
  code: string;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

   private apiUrl = environment.connection.departmentEndpoint;
   private apiKey = environment.connection.apiKey;

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.get<Department[]>(this.apiUrl, { headers});
  }


}
