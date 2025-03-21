import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface Product {
  id: number;     
  name: string;    
  price: number;   
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {


  constructor(private http: HttpClient) {}

private apiUrl = environment.apiUrl +'/products' ;

  public getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }
}
