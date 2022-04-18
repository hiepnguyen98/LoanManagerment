using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Customer
    {
        [Key]
        public string Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [Phone]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(100)]
        public string Email { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
