using Data;
using Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries.Transaction
{
    public interface IGetTransactionHistoryQuery
    {
        public Task<List<TransactionHistoryDto>> ExcuseAsync(TransactionCriteria criteria);
    }
    public class GetTransactionHistoryQuery : IGetTransactionHistoryQuery
    {
        public ApplicationDbContext _dbContext;
        public GetTransactionHistoryQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<TransactionHistoryDto>> ExcuseAsync(TransactionCriteria criteria)
        {
            var q = _dbContext.TransactionHistories.Where(w => w.TransactionId == criteria.TransactionId);
            
            return await q.Select(w => new TransactionHistoryDto
            {
                Id = w.Id,
                CreatedDate= w.CreatedDate,
                Description = w.Description,
                PayAmount = w.PayAmount,
                PayType = w.PayType,
                TransactionId = w.TransactionId,
            }).ToListAsync();
        }
    }
}
