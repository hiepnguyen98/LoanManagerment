using Data;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries.Customer
{
    public interface IGetCustomerQuery
    {
        public Task<List<CustomerDto>> ExcuseAsync(CustomerCriteria criteria);
    }
    public class GetCustomerQuery : IGetCustomerQuery
    {
        public ApplicationDbContext _dbContext;
        public GetCustomerQuery(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<CustomerDto>> ExcuseAsync(CustomerCriteria criteria)
        {
            var q = _dbContext.Customers.Where(w => w.UserId == criteria.UserId);
            return await q.Select(w => new CustomerDto
            {
                Id = w.Id,
                UserId = w.UserId,
                Address = w.Address,
                Email = w.Email,
                Name = w.Name,
                PhoneNumber = w.PhoneNumber,
            }).ToListAsync();
        }
    }
    public class CustomerCriteria : CriteriaBase
    {

    }
}
