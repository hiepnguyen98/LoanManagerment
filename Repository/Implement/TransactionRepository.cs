using Data;
using Dtos;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Repository.Implement
{
    public class TransactionRepository : ITransactionRepository
    {
        public ApplicationDbContext _dbContext;
        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(TransactionDto model)
        {
            _dbContext.Transactions.Add(new Data.Entities.Transaction
            {
                Id = Guid.NewGuid().ToString(),
                TransactionType = model.TransactionType,
                Amount = model.Amount,
                CreatedDate = DateTime.Now,
                CustomerId = model.CustomerId,
                UserId = model.UserId,
                Description = model.Description,
                DueDate = DateTime.Now,
                InterestRate = model.InterestRate,
                InterestAmount = model.InterestAmount,
                InterestType = model.InterestType,
                IsPay = false
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var tran = _dbContext.Transactions.FirstOrDefault(w => w.Id == id);
            if (tran == null)
            {
                throw new Exception("Transaction does not exist");
            }
            else
            {
                _dbContext.Transactions.Remove(tran);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(TransactionDto model)
        {
            var tran = _dbContext.Transactions.FirstOrDefault(w => w.Id == model.Id);
            if (tran == null)
            {
                throw new Exception("Transaction does not exist");
            }
            else
            {
                tran.TransactionType = model.TransactionType;           
                tran.CreatedDate = DateTime.Now;
                tran.CustomerId = model.CustomerId;
                tran.UserId = model.UserId;
                tran.Description = model.Description;
                tran.DueDate = DateTime.Now;
                tran.InterestRate = model.InterestRate;
                tran.InterestAmount = model.InterestAmount;
                tran.InterestType = model.InterestType;
                tran.IsPay = false;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateDueAmount(string tranId){
            var tran = _dbContext.Transactions.FirstOrDefault(w => w.Id == tranId);
            if (tran == null)
            {
                throw new Exception("Transaction does not exist");
            }
            var tranHis = _dbContext.TransactionHistories.Where(w => w.TransactionId == tranId && w.PayType == Infrastructure.Enums.PayType.Installment);
            
            if (tranHis != null)
            {
                var PaidAmount = await tranHis.SumAsync(x => x.PayAmount);
                tran.DueAmount = tran.Amount - PaidAmount;
                tran.IsPay = tran.DueAmount == 0;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task SetIsPay(string tranId){
            var tran = _dbContext.Transactions.FirstOrDefault(w => w.Id == tranId);
            if (tran == null)
            {
                throw new Exception("Transaction does not exist");
            }
            tran.IsPay = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
