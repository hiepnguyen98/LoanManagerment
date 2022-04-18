using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class TransactionHistory
    {
        [Key]
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public Transaction Transaction { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public decimal PayAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime InterestPayForMonth { get; set; }
        public PayType PayType { get; set; }
    }
}
