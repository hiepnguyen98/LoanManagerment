using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class TransactionDto
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public float InterestRate { get; set; }
        public decimal InterestAmount { get; set; }///for dayly Interest
        public InterestType InterestType { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public bool IsPay { get; set; }
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public CustomerDto Customer { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<TransactionHistoryDto> TransactionHistories{get;set;}

    }
}
