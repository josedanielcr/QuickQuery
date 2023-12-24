using API.Dtos;

namespace API.Contracts
{
    public class LoginResultRequest
    {
        public string Token { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public UserDto User { get; set; } = new UserDto();
    }
}
