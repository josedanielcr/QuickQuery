using API.Dtos;

namespace API.Contracts
{
    public class LoginResultRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
