using Dtos;

namespace Client.Services.ApiServices
{
    public class TransactionHistoryService : ApiServiceBase
    {
        public TransactionHistoryService(HttpClient client) : base(client) { }
        public async Task<BaseModelResponseDto<List<TransactionHistoryDto>>> GetTransactionHistoryAsync(string transactionId) => await GetAsync<BaseModelResponseDto<List<TransactionHistoryDto>>>($"api/TransactionHistory/list/{transactionId}");
        public async Task<BaseModelResponseDto> AddTransactionHistoryAsync(TransactionHistoryDto model) => await PostAsync<BaseModelResponseDto, TransactionHistoryDto>("api/TransactionHistory/create", model);
        public async Task<BaseModelResponseDto> UpdateTransactionHistoryAsync(TransactionHistoryDto model) => await PostAsync<BaseModelResponseDto, TransactionHistoryDto>("api/TransactionHistory/update", model);
        public async Task<BaseModelResponseDto> DeleteTransactionHistoryAsync(string id) => await PostAsync<BaseModelResponseDto>($"api/TransactionHistory/{id}/delete");
    }
}
