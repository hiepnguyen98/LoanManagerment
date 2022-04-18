using Dtos;
using Infrastructure.Enums;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Queries.Transaction;
using Repository.Interface;

namespace IdentityProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        public readonly IGetTransactionQuery _getTransactionQuery;
        public readonly IGetTransactionItemQuery _getTransactionItemQuery;
        public readonly ITransactionRepository _transactionRepository;

        public TransactionController(IGetTransactionQuery getTransactionQuery, ITransactionRepository transactionRepository, IGetTransactionItemQuery getTransactionItemQuery)
        {
            _getTransactionQuery = getTransactionQuery;
            _transactionRepository = transactionRepository;
            _getTransactionItemQuery = getTransactionItemQuery;
        }

        [HttpGet]
        [Route("{transactionType}/list")]
        public async Task<IActionResult> GetTransactionsAsync(TransactionType transactionType)
        {
            string userId = User.GetLoggedInUserId<string>();
            var data = await _getTransactionQuery.ExcuseAsync(new TransactionCriteria { UserId = userId, TransactionType = transactionType });
            return Ok(new BaseModelResponseDto<List<TransactionDto>>
            {
                Code = Infrastructure.Enums.ApiResponseCode.Success,
                Message = "get transaction successfully",
                Data = data
            });
            
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTransactionAsync(TransactionDto model)
        {
            try
            {
                model.UserId = User.GetLoggedInUserId<string>();
                await _transactionRepository.Create(model);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Create transaction successfully"
                });
            }catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Create transaction error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateTransactionAsync(TransactionDto model)
        {
            try
            {
                await _transactionRepository.Update(model);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Update transaction successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Update transaction error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("{transactionId}/delete")]
        public async Task<IActionResult> DeleteTransactionAsync(string transactionId)
        {
            try
            {
                await _transactionRepository.Delete(transactionId);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Delete transaction successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Delete transaction error: {ex.Message}"
                });
            }
        }

        [HttpGet]
        [Route("transaction/{transactionId}")]
        public async Task<TransactionDto> GetTransactionAsync(string transactionId)
        {
            return await _getTransactionItemQuery.ExcuseAsync(new TransactionCriteria{TransactionId = transactionId});
        }
    }
}
