namespace Client.Services.ApiServices
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public GetDataDemo GetDataDemo { get; private set;}
        public CustomerService CustomerService { get; private set;}
        public TransactionService TransactionService { get; private set;}
        public TransactionHistoryService TransactionHistoryService { get; private set;}
        public ApiService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            GetDataDemo = new GetDataDemo(httpClient);
            CustomerService = new CustomerService(httpClient);
            TransactionService = new TransactionService(httpClient);
            TransactionHistoryService = new TransactionHistoryService(httpClient);
        }

        public string GetBaseUrl()
        {
            return _httpClient.BaseAddress.ToString();
        }
    }
}
