import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environments';

export interface Product {
    id: number;
    code: string;
    description: string;
    price: number;
    status: boolean;
    departmentId?: number;
    department?: {
        id: number;
        code: string;
        description: string;
    }
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private apiUrl = environment.connection.productEndpoint;
  private apiKey = environment.connection.apiKey;

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.get<Product[]>(this.apiUrl, {headers});
  }

  getProductById(id: number): Observable<Product> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.get<Product>(`${this.apiUrl}/${id}`, {headers});
  }

  createProduct(product: Product): Observable<Product> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.post<Product>(this.apiUrl, product, {headers}).pipe(
      catchError(error => {
        let errorMessage = 'Erro desconhecido!';
        if (error.status != 201) {
          errorMessage = error.error;
        }
        alert(errorMessage);
        return throwError(errorMessage);
      })
    );
  }

  updateProduct(id: number, product: Product): Observable<void> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.put<void>(`${this.apiUrl}/${id}`, product, {headers}).pipe(
      catchError(error => {
        let errorMessage = 'Erro desconhecido!';
        if (error.status != 204) {
          errorMessage = error.error;
        }
        alert(errorMessage);
        return throwError(errorMessage);
      })
    );
  }

  deleteProduct(id: number): Observable<void> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {headers});
  }

}
