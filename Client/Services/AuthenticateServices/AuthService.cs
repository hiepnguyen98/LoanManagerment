using Blazored.LocalStorage;
using Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Client.Services.AuthenticateServices
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<BaseModelResponseDto> Register(UserDto registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/users/login", registerModel);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<BaseModelResponseDto>();
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

        public async Task<BaseModelResponseDto<string>> Login(UserDto loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var res = await _httpClient.PostAsJsonAsync($"api/users/login", loginModel);

            var loginResult = new BaseModelResponseDto<string>();

            if (res.IsSuccessStatusCode)
            {
                loginResult = await res.Content.ReadAsAsync<BaseModelResponseDto<string>>();
            }
            else
            {
                throw new Exception(res.ReasonPhrase);
            }

            if (loginResult.Code != Infrastructure.Enums.ApiResponseCode.Success)
            {
                return loginResult;
            }

            await _localStorage.SetItemAsync("authToken", loginResult.Data);
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Data);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
