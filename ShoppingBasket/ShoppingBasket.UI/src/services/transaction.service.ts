// src/app/services/transaction.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface BasketItemDTO {
  productName: string; // Name of the product
  quantity: number;    // Quantity of the product
  price: number;       // Price of the product
}

export interface DiscountDTO {
  itemName: string;       // Name of the discounted item
  description: string;    // Description of the discount
  discountAmount: number; // Amount of the discount
}

export interface TransactionDTO {
  transactionDate: Date; // Date and time of the transaction
  totalAmount: number;   // Total amount of the transaction
  items: BasketItemDTO[]; // Items in the transaction
  discounts: DiscountDTO[]; // Discounts applied
}

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private apiUrl = environment.apiUrl + '/transactions';

  constructor(private http: HttpClient) {}

  // Fetch all transactions
  getAllTransactions(): Observable<TransactionDTO[]> {
    return this.http.get<TransactionDTO[]>(this.apiUrl);
  }

   // Delete all transactions
   deleteAllTransactions(): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete-all`);
  }
}