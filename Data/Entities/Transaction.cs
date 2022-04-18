using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public float InterestRate { get; set; } //for monthly and yearly Interest
        public decimal InterestAmount { get; set; }///for dayly Interest
        public InterestType InterestType { get; set; }
        public TransactionType TransactionType { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public bool IsPay { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string CustomerId { get; set; } 
        public virtual Customer Customer { get; set; }
        public List<TransactionHistory> TransactionHistories { get; set; }

    }
}
