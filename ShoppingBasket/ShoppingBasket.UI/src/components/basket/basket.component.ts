// src/app/components/basket/basket.component.ts
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { BasketService } from '../../services/basket.service';
import { BasketResponse, BasketItem } from '../../services/basket.service';
import { Product } from '../../services/product.service';
import { TransactionsComponent } from '../transactions/transactions.component'; // Import TransactionsComponent

interface Discount {
  itemName: string;
  description: string;
  discountAmount: number;
}

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, FormsModule, TransactionsComponent], // Add TransactionsComponent here
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent implements OnInit {
  products: Product[] = [];
  basketItems: BasketItem[] = [];
  discounts: Discount[] = [];
  subtotal = 0;
  total = 0;
  receipt: string = '';

    // Toggle transactions visibility
    showTransactions = false;

  constructor(
    private productService: ProductService,
    private basketService: BasketService
  ) {}

  ngOnInit() {
    this.loadProducts();
  }

  // Load products
  loadProducts() {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
      this.basketItems = this.products.map(p => ({
        productId: p.id,
        quantity: 0
      }));
      console.log("basketItems after initialization:", this.basketItems);
    });
  }

  // Calculate total
  calculateTotal() {
    const itemsToSend = this.basketItems.filter(item => item.quantity > 0);
    this.receipt = '';
    if (itemsToSend.length === 0) {
      alert("Please add at least one item to the basket.");
      return;
    }

    console.log("Payload sent to backend:", itemsToSend);

    this.basketService.calculateTotal(itemsToSend).subscribe(
      (response: BasketResponse) => {
        this.subtotal = response.subtotal;
        this.total = response.total;
        this.discounts = response.discounts;
      },
      (error: any) => {
        console.error("Error calculating total:", error);
      }
    );
  }

  // Generate receipt
  generateReceipt() {
    const itemsToSend = this.basketItems.filter(item => item.quantity > 0);

    this.basketService.generateReceipt(itemsToSend).subscribe(
      (response: { receipt: string }) => {
        this.receipt = response.receipt;
      },
      (error: any) => {
        console.error("Error generating receipt:", error);
      }
    );
  }
    // Toggle transactions visibility
    toggleTransactions() {
      this.showTransactions = !this.showTransactions;
    }
    // Clear basket and receipt
  clearBasket() {
    if (confirm('Are you sure you want to clear the basket and receipt? This action cannot be undone.')) {
      // Reset basket items
      this.basketItems = this.products.map(p => ({
        productId: p.id,
        quantity: 0
      }));

      // Reset totals and discounts
      this.subtotal = 0;
      this.total = 0;
      this.discounts = [];

      // Clear receipt
      this.receipt = '';

      console.log('Basket and receipt cleared successfully');
    }
  }
}