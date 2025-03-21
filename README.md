# Shopping-Basket-Application
Develop a full-stack application to manage a shopping basket, calculate the total price of grocery items, apply discounts, and display a detailed receipt.



# project structure



# api

# core


# data



# test

falar dos packages usados dos padroes a ser usados e porque

# Shopping Basket Project Documentation

## Overview
The **Shopping Basket** project is a .NET-based application that simulates an e-commerce shopping basket system. It allows users to add items to their basket, calculate subtotals, apply discounts, and generate receipts. The project is designed with a focus on modularity, testability, and clean architecture.

---

## Project Structure

The project is organized into the following key components:

### 1. **Core Layer**
This layer contains the core business logic of the application. It includes models, services, and interfaces.

#### Key Components:
- **Models**: Represent the data structures used in the application.
  - `BasketItem`: Represents an item in the shopping basket.
  - `Product`: Represents a product with properties like `Id`, `Name`, and `Price`.
  - `Discount`: Represents a discount applied to the basket.
  - `Transaction`: Represents a completed transaction with details like `TransactionDate`, `TotalAmount`, and applied discounts.

- **Services**: Contain the business logic for the shopping basket and discounts.
  - `BasketService`: Handles basket operations like calculating subtotals, totals, and generating receipts.
  - `DiscountService`: Handles discount calculations based on predefined rules.

- **Interfaces**: Define contracts for repositories and services.
  - `IProductRepository`: Provides methods for accessing product data.
  - `IDiscountRepository`: Provides methods for accessing discount data.
  - `ITransactionRepository`: Provides methods for saving transactions.
  - `IBasketService`: Defines the contract for basket operations.
  - `IDiscountService`: Defines the contract for discount calculations.

---

### 2. **Data Layer**
This layer is responsible for data access and persistence. It includes repositories that interact with the database.

#### Key Components:
- **Repositories**:
  - `ProductRepository`: Implements `IProductRepository` to fetch product details.
  - `DiscountRepository`: Implements `IDiscountRepository` to fetch and manage discounts.
  - `TransactionRepository`: Implements `ITransactionRepository` to save transaction data.

---

### 3. **Tests Layer**
This layer contains unit tests for the core services to ensure correctness and reliability.

#### Key Components:
- **BasketService Tests**:
  - Test methods for `CalculateSubtotalAsync`, `CalculateTotalAsync`, and `GenerateReceiptAsync`.
  - Edge cases like empty baskets, invalid product IDs, and discounts.

- **DiscountService Tests**:
  - Test methods for `CalculateDiscountsAsync`.
  - Scenarios like apple discounts, multi-buy discounts (e.g., buy 2 soups, get 1 bread at half price).

---

## How Components Interact

### Workflow Overview
1. **User Adds Items to Basket**:
   - The user adds products to the basket, represented as a list of `BasketItem` objects.

2. **Calculate Subtotal**:
   - The `BasketService` calculates the subtotal by fetching product prices from the `ProductRepository` and multiplying them by the quantities.

3. **Apply Discounts**:
   - The `DiscountService` calculates applicable discounts based on predefined rules (e.g., 10% off apples, buy 2 soups get 1 bread at half price).
   - Discounts are fetched or created using the `DiscountRepository`.

4. **Calculate Total**:
   - The `BasketService` subtracts the total discount amount from the subtotal to get the final total.

5. **Generate Receipt**:
   - The `BasketService` generates a receipt showing the subtotal, discounts, and total.
   - The transaction is saved to the database using the `TransactionRepository`.

---

### Interaction Diagram

```plaintext
User -> BasketService: Add items to basket
BasketService -> ProductRepository: Fetch product details
BasketService -> DiscountService: Calculate discounts
DiscountService -> DiscountRepository: Fetch/create discounts
BasketService -> TransactionRepository: Save transaction
BasketService -> User: Return receipt


missing frontend