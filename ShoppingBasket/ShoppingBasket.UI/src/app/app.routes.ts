import { Routes } from '@angular/router';
import { BasketComponent } from '../components/basket/basket.component';
import { ReceiptComponent } from '../components/receipt/receipt.component';
import { TransactionsComponent } from '../components/transactions/transactions.component';
export const routes: Routes = [
  { path: '', redirectTo: 'basket', pathMatch: 'full' }, // Default route
  { path: 'basket', component: BasketComponent },
  { path: 'receipt', component: ReceiptComponent },
  { path: 'transactions', component: TransactionsComponent },
];