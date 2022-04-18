using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Queries.Customer;
using Queries.Transaction;
using Repository.Interface;
using Infrastructure.Extensions;

namespace IdentityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        public readonly IGetCustomerQuery _getCustomerQuery;
        public readonly ICustomerRepository _customerRepo;

        public CustomerController(IGetCustomerQuery getCustomerQuery, ICustomerRepository customerRepo)
        {
            _getCustomerQuery = getCustomerQuery;
            _customerRepo = customerRepo;
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetCustomerAsync()
        {
            string userId = User.GetLoggedInUserId<string>();
            var data = await _getCustomerQuery.ExcuseAsync(new CustomerCriteria { UserId = userId });
            return Ok(new BaseModelResponseDto<List<CustomerDto>>
            {
                Code = Infrastructure.Enums.ApiResponseCode.Success,
                Message = "get transaction successfully",
                Data = data
            });

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCustomerAsync(CustomerDto model)
        {
            try
            {
                string userId = User.GetLoggedInUserId<string>();
                model.UserId = userId;
                await _customerRepo.Create(model);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Create customer successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Create customer error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateCustomerAsync(CustomerDto model)
        {
            try
            {
                await _customerRepo.Update(model);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Update customer successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Update customer error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("{customerId}/delete")]
        public async Task<IActionResult> DeleteCustomerAsync(string customerId)
        {
            try
            {
                await _customerRepo.Delete(customerId);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Delete customer successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Delete customer error: {ex.Message}"
                });
            }
        }
    }
}
