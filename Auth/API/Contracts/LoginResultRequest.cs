using QuickqueryAuthenticationAPI.Dtos;

namespace QuickqueryAuthenticationAPI.Contracts
{
    public class LoginResultRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
