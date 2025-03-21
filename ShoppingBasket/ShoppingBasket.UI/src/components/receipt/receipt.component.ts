import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-receipt',
  standalone: true,
  imports: [CommonModule], // Import CommonModule
  templateUrl: './receipt.component.html',
  styleUrls: ['./receipt.component.css']
})
export class ReceiptComponent {
  @Input() receipt: string = '';
}