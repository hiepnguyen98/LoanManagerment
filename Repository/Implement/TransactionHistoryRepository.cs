using Data;
using Dtos;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implement
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        public ApplicationDbContext _dbContext;
        public TransactionHistoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(TransactionHistoryDto model)
        {
            _dbContext.TransactionHistories.Add(new Data.Entities.TransactionHistory
            {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = model.CreatedDate,
                Description = model.Description,
                PayAmount = model.PayAmount,
                PayType = model.PayType,
                TransactionId = model.TransactionId,
                InterestPayForMonth = model.InterestPayForMonth,
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var tranHis = _dbContext.TransactionHistories.FirstOrDefault(w => w.Id == id);
            if (tranHis == null)
            {
                throw new Exception("Transaction History does not exist");
            }
            else
            {
                _dbContext.TransactionHistories.Remove(tranHis);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(TransactionHistoryDto model)
        {
            var tran = _dbContext.TransactionHistories.FirstOrDefault(w => w.Id == model.Id);
            if (tran == null)
            {
                throw new Exception("Transaction History does not exist");
            }
            else
            {
                tran.CreatedDate = model.CreatedDate;
                tran.Description = model.Description;
                tran.PayAmount = model.PayAmount;
                tran.PayType = model.PayType;
                tran.TransactionId = model.TransactionId;
                tran.InterestPayForMonth = model.InterestPayForMonth;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
