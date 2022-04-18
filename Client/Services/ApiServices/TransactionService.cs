using Dtos;
using Infrastructure.Enums;

namespace Client.Services.ApiServices
{
    public class TransactionService : ApiServiceBase
    {
        public TransactionService(HttpClient client) : base(client) { }
        public async Task<BaseModelResponseDto<List<TransactionDto>>> GetTransactionAsync(TransactionType transactionType) => await GetAsync<BaseModelResponseDto<List<TransactionDto>>>($"api/Transaction/{transactionType}/list");
        public async Task<BaseModelResponseDto> AddTransactionAsync(TransactionDto model) => await PostAsync<BaseModelResponseDto, TransactionDto>("api/Transaction/create", model);
        public async Task<BaseModelResponseDto> UpdateTransactionAsync(TransactionDto model) => await PostAsync<BaseModelResponseDto, TransactionDto>("api/Transaction/update", model);
        public async Task<BaseModelResponseDto> DeleteTransactionAsync(string id) => await PostAsync<BaseModelResponseDto>($"api/Transaction/{id}/delete");
        public async Task<TransactionDto> GetTransactionItemAsync(string transactionId) => await GetAsync<TransactionDto>($"api/Transaction/transaction/{transactionId}");
    }
}
