import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { BasketService } from '../../services/basket.service';
import { BasketResponse, BasketItem } from '../../services/basket.service';
import { Product } from '../../services/product.service';

interface Discount {
  itemName: string;
  description: string;
  discountAmount: number;
}

@Component({
  selector: 'app-basket',
  standalone: true,
  imports: [CommonModule, FormsModule], // Add CommonModule here
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

  constructor(private productService: ProductService, private basketService: BasketService) {}

  ngOnInit() {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
      this.basketItems = this.products.map(p => ({ productId: p.productId, quantity: 0 }));
    });
  }

  calculateTotal() {
    const itemsToSend = this.basketItems.filter(item => item.quantity > 0);

    if (itemsToSend.length === 0) {
      alert("Please add at least one item to the basket.");
      return;
    }

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
}