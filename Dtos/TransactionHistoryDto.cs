using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class TransactionHistoryDto
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public string Description { get; set; }
        public decimal PayAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime InterestPayForMonth { get; set; }
        public PayType PayType { get; set; }
    }
}
