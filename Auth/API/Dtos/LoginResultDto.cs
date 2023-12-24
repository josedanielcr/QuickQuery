namespace API.Dtos
{
    public class LoginResultDto
    {
        public string Token { get; set; } = String.Empty;
        public string RefreshToken { get; set; } = String.Empty;
        public UserDto User { get; set; } = new UserDto();
    }
}