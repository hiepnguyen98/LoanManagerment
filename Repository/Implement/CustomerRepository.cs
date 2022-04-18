using Data;
using Data.Entities;
using Dtos;
using Repository.Interface;

namespace Repository.Implement
{
    public class CustomerRepository : ICustomerRepository
    {
        public ApplicationDbContext _dbContext;
        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(CustomerDto model)
        {
            _dbContext.Customers.Add(new Customer
            {
                Id = Guid.NewGuid().ToString(),
                UserId = model.UserId,
                Address = model.Address,
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var cus = _dbContext.Customers.FirstOrDefault(w => w.Id == id);
            if (cus == null)
            {
                throw new Exception("Customer does not exist");
            }
            else
            {
                _dbContext.Customers.Remove(cus);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(CustomerDto model)
        {
            var tran = _dbContext.Customers.FirstOrDefault(w => w.Id == model.Id);
            if (tran == null)
            {
                throw new Exception("Customer does not exist");
            }
            else
            {
                tran.Address = model.Address;
                tran.Email = model.Email;
                tran.Name = model.Name;
                tran.PhoneNumber = model.PhoneNumber;

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
