import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BasketComponent } from '../components/basket/basket.component';
import { ReceiptComponent } from '../components/receipt/receipt.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [BasketComponent, RouterModule], // Import standalone components
  template: `
    <div class="app-container">
      <app-basket></app-basket>
       <router-outlet></router-outlet> <!-- Add router-outlet -->
    </div>
  `,
  styles: [
    `
      .app-container {
        max-width: 800px;
        margin: 0 auto;
        padding: 20px;
      }
    `
  ]
})
export class AppComponent {}