using Dtos;
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
    public class TransactionHistoryController : ControllerBase
    {
        public readonly IGetTransactionHistoryQuery _getTransactionHistoryQuery;
        public readonly ITransactionHistoryRepository _transactionHistoryRepository;
        public readonly ITransactionRepository _transactionRepository;

        public TransactionHistoryController(IGetTransactionHistoryQuery getTransactionHistoryQuery,
         ITransactionHistoryRepository transactionHistoryRepository,
         ITransactionRepository transactionRepository)
        {
            _getTransactionHistoryQuery = getTransactionHistoryQuery;
            _transactionHistoryRepository = transactionHistoryRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        [Route("list/{tranId}")]
        public async Task<IActionResult> GetTransactionHistoryAsync(string tranId)
        {
            var data = await _getTransactionHistoryQuery.ExcuseAsync(new TransactionCriteria { TransactionId = tranId });
            return Ok(new BaseModelResponseDto<List<TransactionHistoryDto>>
            {
                Code = Infrastructure.Enums.ApiResponseCode.Success,
                Message = "get transaction successfully",
                Data = data
            });

        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTransactionHistoryAsync(TransactionHistoryDto model)
        {
            try
            {
                await _transactionHistoryRepository.Create(model);
                if(model.PayType == Infrastructure.Enums.PayType.Installment){
                    await _transactionRepository.UpdateDueAmount(model.TransactionId);
                }else if(model.PayType == Infrastructure.Enums.PayType.PayAll){
                    await _transactionRepository.SetIsPay(model.TransactionId);
                }
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Create transaction history successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Create transaction history error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateTransactionAsync(TransactionHistoryDto model)
        {
            try
            {
                await _transactionHistoryRepository.Update(model);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Update transaction history successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Update transaction history error: {ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("{transactionHistoryId}/delete")]
        public async Task<IActionResult> DeleteTransactionHistoryAsync(string transactionHistoryId)
        {
            try
            {
                await _transactionHistoryRepository.Delete(transactionHistoryId);
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.Success,
                    Message = "Delete transaction history successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseModelResponseDto
                {
                    Code = Infrastructure.Enums.ApiResponseCode.BadRequest,
                    Message = $"Delete transaction history error: {ex.Message}"
                });
            }
        }
    }
}
