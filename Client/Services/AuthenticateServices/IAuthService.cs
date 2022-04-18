using Dtos;

namespace Client.Services.AuthenticateServices
{
    public interface IAuthService
    {
        public Task<BaseModelResponseDto> Register(UserDto registerModel);
        public Task<BaseModelResponseDto<string>> Login(UserDto registerModel);
        public Task Logout();
    }
}
