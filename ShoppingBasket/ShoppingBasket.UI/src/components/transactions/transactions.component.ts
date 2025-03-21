// src/app/components/transactions/transactions.component.ts
import { Component, OnInit } from '@angular/core';
import { TransactionService } from '../../services/transaction.service';
import { TransactionDTO } from '../../services/transaction.service';
import { CommonModule } from '@angular/common'; // Import CommonModule
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-transactions',
  standalone: true,
  templateUrl: './transactions.component.html',
  imports: [CommonModule, FormsModule], // Add CommonModule here
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {
  transactions: TransactionDTO[] = [];

  constructor(private transactionService: TransactionService) {}

  ngOnInit() {
    this.loadTransactions();
  }

  // Fetch all transactions
  loadTransactions(): void {
    this.transactionService.getAllTransactions().subscribe(
      (data) => {
        this.transactions = data;
        console.log('Transactions loaded:', this.transactions); // Log data to console
      },
      (error) => {
        console.error('Error loading transactions:', error);
      }
    );
  }

  // Delete all transactions
  deleteAllTransactions() {
    if (confirm('Are you sure you want to delete ALL transactions? This action cannot be undone.')) {
      this.transactionService.deleteAllTransactions().subscribe(
        () => {
          // Clear the transactions list
          this.transactions = [];
          console.log('All transactions deleted successfully');
          this.loadTransactions(); // Reload transactions
        },
        (error) => {
          console.error('Error deleting all transactions:', error);
        }
      );
    }
  }
}