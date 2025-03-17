using Microsoft.AspNetCore.Mvc;

using ShoppingBasket.Core.Interfaces;

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
        var transactions = await _transactionRepository.GetAllAsync();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransactionById(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();
        return Ok(transaction);
    }
}