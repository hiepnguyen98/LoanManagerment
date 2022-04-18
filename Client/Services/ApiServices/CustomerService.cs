using Dtos;

namespace Client.Services.ApiServices
{
    public class CustomerService: ApiServiceBase
    {
        public CustomerService(HttpClient client) : base(client) {  }
        public async Task<BaseModelResponseDto<List<CustomerDto>>> GetCustomerAsync() => await GetAsync<BaseModelResponseDto<List<CustomerDto>>>("api/Customer/list");
        public async Task<BaseModelResponseDto> AddCustomerAsync(CustomerDto model) => await PostAsync<BaseModelResponseDto,CustomerDto>("api/Customer/create", model);
        public async Task<BaseModelResponseDto> UpdateCustomerAsync(CustomerDto model) => await PostAsync<BaseModelResponseDto,CustomerDto>("api/Customer/update", model);
        public async Task<BaseModelResponseDto> DeleteCustomerAsync(string customerId) => await PostAsync<BaseModelResponseDto>($"api/Customer/{customerId}/delete");
    }
}
