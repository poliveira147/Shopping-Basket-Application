import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface BasketItem {
  productId: number;
  quantity: number;
}

export interface BasketResponse {
  subtotal: number;
  discounts: { itemName: string; description: string; discountAmount: number }[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class BasketService {



  constructor(private http: HttpClient) {}
  private apiUrl = environment.apiUrl +'/basket';
  
  calculateTotal(basketItems: BasketItem[]): Observable<BasketResponse> {
    return this.http.post<BasketResponse>(this.apiUrl + '/calculate-total', basketItems);
  }

  generateReceipt(basketItems: BasketItem[]): Observable<{ receipt: string }> {
    return this.http.post<{ receipt: string }>(this.apiUrl + '/generate-receipt', basketItems);
  }
}
