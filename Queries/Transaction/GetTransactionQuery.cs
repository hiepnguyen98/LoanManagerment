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
    public interface IGetTransactionQuery
    {
        public Task<List<TransactionDto>> ExcuseAsync(TransactionCriteria criteria);
    }
    public class GetTransactionQuery : IGetTransactionQuery
    {
        public ApplicationDbContext _dbContext;
        public GetTransactionQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<TransactionDto>> ExcuseAsync(TransactionCriteria criteria)
        {
            var q = _dbContext.Transactions.Include(w=>w.Customer).Include(w=>w.TransactionHistories).Where(w => w.UserId == criteria.UserId && w.TransactionType == criteria.TransactionType);

            if (!string.IsNullOrWhiteSpace(criteria.CustomerId))
            {
                q = q.Where(w => w.CustomerId == criteria.CustomerId);
            }
            return await q.Select(w => new TransactionDto
            {
                Id = w.Id,
                UserId = w.UserId,
                CreatedDate = w.CreatedDate,
                CustomerId = w.CustomerId,
                Amount = w.Amount,
                TransactionType = w.TransactionType,
                Description = w.Description,
                DueDate = w.DueDate,
                InterestRate = w.InterestRate,
                IsPay = w.IsPay,
                UpdatedDate = w.UpdatedDate,
                DueAmount = w.DueAmount,
                InterestAmount = w.InterestAmount,
                InterestType = w.InterestType,
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
                    InterestPayForMonth = x.InterestPayForMonth
                }).OrderByDescending(x => x.CreatedDate).Take(5).ToList()
            }).ToListAsync();
        }
    }

    public class TransactionCriteria : CriteriaBase
    {
        public string TransactionId { get; set; }
        public string CustomerId { get; set; }
        public TransactionType TransactionType { get; set; }  

    }


}
