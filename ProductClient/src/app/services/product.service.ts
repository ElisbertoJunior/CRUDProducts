import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';

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

  private apiUrl = 'http://localhost:5167/api/Products'
  private apiKey = "Produtos 123456789"

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
    return this.http.post<Product>(this.apiUrl, product, {headers});
  }

  updateProduct(id: number, product: Product): Observable<void> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.put<void>(`${this.apiUrl}/${id}`, product, {headers});
  }

  deleteProduct(id: number): Observable<void> {
    const headers = new HttpHeaders({
      'ApiKey': `${this.apiKey}`
    })
    return this.http.delete<void>(`${this.apiUrl}/${id}`, {headers});
  }

}
