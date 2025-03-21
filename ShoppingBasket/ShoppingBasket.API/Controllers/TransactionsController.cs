using Microsoft.AspNetCore.Mvc;

using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.DTOs;
using ShoppingBasket.Core.Models;
[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionsController(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTransactions()
    {
        try
        {
            // Fetch all transactions from the database
            var transactions = await _transactionRepository.GetAllAsync();

            // Map transactions to TransactionResponseDTO
            var transactionDTOs = transactions.Select(transaction => new TransactionDTO
            {
                TransactionDate = transaction.TransactionDate,
                TotalAmount = transaction.TotalAmount,
                Items = transaction.Items.Select(item => new BasketItemDTO
                {
                    ProductName = item.Product?.Name, // Use the product name
                    Quantity = item.Quantity,
                    Price = item.Product?.Price ?? 0 // Use the product price
                }).ToList(),
                Discounts = transaction.Discounts.Select(discount => new DiscountDTO
                {
                    ItemName = discount.ItemName,
                    Description = discount.Description, // Use the discount description
                    DiscountAmount = discount.DiscountAmount
                }).ToList()
            }).ToList();

            return Ok(transactionDTOs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        try
        {
            // Fetch the transaction from the database
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            // Map the Transaction model to a TransactionResponseDTO
            var transactionDTO = new TransactionDTO
            {
                TransactionDate = transaction.TransactionDate,
                TotalAmount = transaction.TotalAmount,
                Items = transaction.Items.Select(item => new BasketItemDTO
                {
                    ProductName = item.Product?.Name, // Use the product name
                    Quantity = item.Quantity,
                    Price = item.Product?.Price ?? 0 // Use the product price
                }).ToList(),
                Discounts = transaction.Discounts.Select(discount => new DiscountDTO
                {
                    Description = discount.Description, // Use the discount description
                    DiscountAmount = discount.DiscountAmount
                }).ToList()
            };

            return Ok(transactionDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
        }
    }
    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAllTransactions()
    {
        try
        {
            await _transactionRepository.DeleteAllAsync();
            return NoContent(); // 204 No Content
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { errorCode = "INTERNAL_ERROR", message = ex.Message });
        }
    }
}