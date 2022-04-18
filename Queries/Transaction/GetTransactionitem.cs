using Data;
using Dtos;
using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries.Transaction
{
    public interface IGetTransactionItemQuery
    {
        public Task<TransactionDto> ExcuseAsync(TransactionCriteria criteria);
    }
    public class GetTransactionItemQuery : IGetTransactionItemQuery
    {
        public ApplicationDbContext _dbContext;
        public GetTransactionItemQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TransactionDto> ExcuseAsync(TransactionCriteria criteria)
        {
            var w = await _dbContext.Transactions.Include(w=>w.Customer).Include(w=>w.TransactionHistories).Where(w => w.Id == criteria.TransactionId).FirstOrDefaultAsync();
            if(w == null){
                return null;
            }
            return new TransactionDto
            {
                Id = w.Id,
                UserId = w.UserId,
                CreatedDate = w.CreatedDate,
                CustomerId = w.CustomerId,
                Amount = w.Amount,
                DueAmount = w.DueAmount,
                TransactionType = w.TransactionType,
                Description = w.Description,
                DueDate = w.DueDate,
                InterestRate = w.InterestRate,
                IsPay = w.IsPay,
                UpdatedDate = w.UpdatedDate,
                Customer = w.Customer == null ? null : new CustomerDto
                {
                    Id = w.Customer.Id,
                    UserId = w.Customer.UserId,
                    Address = w.Customer.Address,
                    Email = w.Customer.Email,
                    Name = w.Customer.Name,
                    PhoneNumber = w.Customer.PhoneNumber,
                },
                CustomerEmail = w.Customer == null ? null : w.Customer.Email,
                CustomerName = w.Customer == null ? null : w.Customer.Name,
                TransactionHistories = w.TransactionHistories == null ? new List<TransactionHistoryDto>() : w.TransactionHistories.Select(x => new TransactionHistoryDto{
                    Id = x.Id,
                    CreatedDate= x.CreatedDate,
                    Description = x.Description,
                    PayAmount = x.PayAmount,
                    PayType = x.PayType,
                    TransactionId = x.TransactionId,
                }).Take(5).ToList()
            };
        }
    }


}
