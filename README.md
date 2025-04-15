
## Overview

The  **Shopping Basket**  project is a full-stack application built with  **.NET for the backend**  and  **Angular for the frontend**. It simulates an e-commerce shopping basket system where users can add items, calculate totals, apply discounts, and generate receipts. The project is designed with a focus on modularity, testability, and clean architecture.

----------

## Solution Structure

The solution is divided into two main parts:

1.  **Backend**: Built with .NET, responsible for business logic, data access, and API endpoints.
    
2.  **Frontend**: Built with Angular, responsible for the user interface and interaction.
    

----------

### Backend Structure

#### 1.  **Core Layer**

This layer contains the core business logic of the application. It includes models, services, and interfaces.

##### Key Components:

-   **Models**:
    
    -   `BasketItem`: Represents an item in the shopping basket.
        
    -   `Product`: Represents a product with properties like  `Id`,  `Name`, and  `Price`.
        
    -   `Discount`: Represents a discount applied to the basket.
        
    -   `Transaction`: Represents a completed transaction with details like  `TransactionDate`,  `TotalAmount`, and applied discounts.
        
-   **Services**:
    
    -   `BasketService`: Handles basket operations like calculating subtotals, totals, and generating receipts.
        
    -   `DiscountService`: Handles discount calculations based on predefined rules.
        
-   **Interfaces**:
    
     -   `IBasketRepository`: Provides methods for accessing basket items data.
      
    -   `IProductRepository`: Provides methods for accessing product data.
        
    -   `IDiscountRepository`: Provides methods for accessing discount data.
        
    -   `ITransactionRepository`: Provides methods for saving transactions.
        
    -   `IBasketService`: Defines the contract for basket operations.
        
    -   `IDiscountService`: Defines the contract for discount calculations.
        

#### 2.  **Data Layer**

This layer is responsible for data access and persistence. It includes repositories that interact with the database.

##### Key Components:

-   **Repositories**:
    -   `BasketRepository`: Implements  `IBasketRepository`  to fetch basket items.
    
    -   `ProductRepository`: Implements  `IProductRepository`  to fetch product details.
        
    -   `DiscountRepository`: Implements  `IDiscountRepository`  to fetch and manage discounts.
        
    -   `TransactionRepository`: Implements  `ITransactionRepository`  to save transaction data.

-    **Database**:
    -    The Current Database is SQL Lite.
         
#### 3.  **API Layer**

This layer exposes RESTful endpoints for the frontend to interact with the backend.

##### Key Endpoints:

-   `GET /api/products`: Fetch all available products.
    
-   `POST /api/basket/calculate-total`: Calculate the total price of the basket.
    
-   `POST /api/basket/generate-receipt`: Generate a receipt for the current basket.
    
-   `GET /api/transactions`: Fetch all transactions.
    
-   `DELETE /api/transactions/delete-all`: Delete all transactions.
    

#### 4.  **Tests Layer**

This layer contains **Unit tests** for the core services and  **GUI tests**  to ensure correctness and reliability. The tests are divided into two categories:

1.  **Unit Tests**: Focus on testing individual components and services in isolation.
    
2.  **GUI Tests**: Focus on testing the frontend user interface and its interaction with the backend.
    

----------

#### **1. Unit Tests**

Unit tests are written for the core services to ensure that the business logic works as expected. These tests use  **MSTest**  as the testing framework and  **Moq**  for mocking dependencies.

##### **Key Tests:**

-   **BasketService Tests**:
    
    -   Test methods for  `CalculateSubtotalAsync`,  `CalculateTotalAsync`, and  `GenerateReceiptAsync`.
        
    -   Edge cases like empty baskets, invalid product IDs, and discounts.
        
-   **DiscountService Tests**:

-   Test methods for  `CalculateDiscountsAsync`.
    
-   Scenarios like apple discounts (10% off) and multi-buy discounts (e.g., buy 2 soups, get 1 bread at half price).
        
#### **2. GUI Tests**

GUI tests are written to ensure that the frontend user interface behaves as expected and interacts correctly with the backend. These tests are implemented using  **Selenium**.

##### **Key Tests:**

-   **BasketComponent Tests**:
    
    -   Verify that the basket displays the correct items and quantities.
        
    -   Test the "Calculate Total" and "Generate Receipt" buttons to ensure they interact with the backend and display the correct results.

**TransactionsComponent Tests**:

-   Verify that the transactions page displays all saved transactions.
    
-   Test the "Delete All Transactions" button to ensure it clears the transaction history.

----------

### Frontend Structure

The frontend is built with  **Angular**  and follows a modular structure. It communicates with the backend via RESTful APIs.

#### 1.  **Components**

-   **BasketComponent**:
    
    -   Displays the list of products.
        
    -   Allows users to add items to the basket, calculate totals, and generate receipts.
        
    -   Includes buttons to refresh the basket, clear the basket, and view transactions.
        
-   **TransactionsComponent**:
    
    -   Displays a list of all transactions.
        
    -   Allows users to delete all transactions.

-   **ReceiptComponent**:
    
    -   Displays the receipt with the price and all the discounts.

#### 2.  **Services**

-   **ProductService**:
    
    -   Fetches product data from the backend.
        
-   **BasketService**:
    
    -   Handles basket-related operations like calculating totals and generating receipts.
        
-   **TransactionService**:
    
    -   Fetches and deletes transactions.
        

#### 3.  **Routing**

-   The application uses Angular's router to navigate between the basket and transactions views.
    
-   Routes:
    
    -   `/`: Default route, displays the  `BasketComponent`.
        
    -   `/transactions`: Displays the  `TransactionsComponent`.
        

#### 4.  **Styling**

-   **Bootstrap**: Used for responsive design and pre-built components.
    
-   **Custom CSS**: Added for specific styling needs (e.g., scrollable transaction list, button styles).
    

#### 5.  **State Management**

-   Angular's built-in  **RxJS**  is used for state management.
    
-   Services like  `BasketService`  and  `TransactionService`  maintain the state of the basket and transactions.
    

----------

## How Components Interact

### Workflow Overview

1.  **User Adds Items to Basket**:
    
    -   The user adds products to the basket, represented as a list of  `BasketItem`  objects.
        
2.  **Calculate Subtotal**:
    
    -   The  `BasketService`  calculates the subtotal by fetching product prices from the  `ProductRepository`  and multiplying them by the quantities.
        
3.  **Apply Discounts**:
    
    -   The  `DiscountService`  calculates applicable discounts based on predefined rules (e.g., 10% off apples, buy 2 soups get 1 bread at half price).
        
    -   Discounts are fetched or created using the  `DiscountRepository`.
        
4.  **Calculate Total**:
    
    -   The  `BasketService`  subtracts the total discount amount from the subtotal to get the final total.
        
5.  **Generate Receipt**:
    
    -   The  `BasketService`  generates a receipt showing the subtotal, discounts, and total.
        
    -   The transaction is saved to the database using the  `TransactionRepository`.
        
6.  **Frontend Interaction**:
    
    -   The frontend communicates with the backend via RESTful APIs.
        
    -   The  `BasketComponent`  and  `TransactionsComponent`  fetch and display data from the backend.
        

----------

### Interaction Diagram



User -> Frontend (Angular): Interacts with UI

Frontend -> Backend (API): Sends HTTP requests

Backend -> Core Layer: Processes business logic

Core Layer -> Data Layer: Fetches/saves data

Backend -> Frontend: Returns response

Frontend -> User: Displays results

----------

## Packages and Tools Used

### Backend (.NET)

-   **ASP.NET Core**: Framework for building RESTful APIs.
    
-   **Entity Framework Core**: ORM for database interactions.
    
-   **MSTest**: Testing framework for unit tests.
    
-   **Moq**: Mocking library for unit tests.
    
-   **Swagger**: API documentation tool.
    

### Frontend (Angular)

-   **Angular CLI**: Tool for scaffolding and managing Angular projects.
    
-   **RxJS**: Library for reactive programming and state management.
    
-   **Bootstrap**: CSS framework for responsive design.
-  **SELENIUM**: Testing framework for UI tests.
   
    

----------

## Why This Structure?

1.  **Separation of Concerns**:
    
    -   The backend is divided into layers (Core, Data, API) to separate business logic, data access, and API endpoints.
        
    -   The frontend is modular, with components, services, and routing clearly separated.
        
2.  **Testability**:
    
    -   The use of interfaces and dependency injection makes the application easy to test.
        
    -   Unit tests are written for core services to ensure reliability.
        
3.  **Scalability**:
    
    -   The modular structure allows for easy addition of new features (e.g., make discount dynamic).
        
4.  **Maintainability**:
    
    -   Clean architecture and clear separation of layers make the codebase easy to maintain.
        

----------

## Future Improvements

1.  **Authentication and Authorization**:
    
    -   Add user authentication to allow multiple users to manage their own baskets.
        
2.  **Database Integration**:
    
    -   Use a real database (e.g., SQL Server, PostgreSQL) instead of in-memory data.
        
3.  **Frontend Enhancements**:
    
    -   Better UI
    -  Better position of UI components
    -  More User-Friendly buttons
    -  Login/Logout
    
4.  **Code Related**:
    
    -   Bigger test coverage
    -  More validations on code (nulls, ...)
    -  Make discounts dynamic
    - API Versioning

     ## How to Run the Application

### Backend

1.  **Prerequisites**:
    
    -   .NET SDK installed.
        
    -   IDE (e.g., Visual Studio, Visual Studio Code).
        
2.  **Steps**:
    
    -   Open the backend project in your IDE.
        
    -   Restore the NuGet packages.
        
    -   Set the  `ShoppingBasket.API`  project as the startup project.
        
    -   Run the application. The API should start and be accessible at  `http://localhost:7206`
        
3.  **Swagger**:
    - To run with swagger uncomment the swagger related lines on the program.cs 
    
    -   Navigate to  `http://localhost:5000/swagger`  to view and test the API endpoints.
        

### Frontend

1.  **Prerequisites**:
    
    -   Node.js and npm installed.
        
    -   Angular CLI installed globally (`npm install -g @angular/cli`).
        
2.  **Steps**:
    
    -   Open the frontend project in your IDE.
        
    -   Run  `npm install`  to install the required packages.
        
    -   Run  `ng serve --port 65089`  to start the Angular development server.
        
    -   Navigate to  `http://localhost:65089/basket`  to view the application.
