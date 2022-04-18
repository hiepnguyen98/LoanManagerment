using Blazored.LocalStorage;

namespace Client.Services.AuthenticateServices
{
    public class CustomApiAuthorizationHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;
        public CustomApiAuthorizationHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
