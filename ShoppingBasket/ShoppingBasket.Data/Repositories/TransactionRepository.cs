﻿
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Core.Interfaces;
using ShoppingBasket.Core.Models;
using ShoppingBasket.Data.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Data.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly ShoppingBasketDbContext _context;

        public TransactionRepository(ShoppingBasketDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task AddAsync(Transaction entity)
        {
            await _context.Transactions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
