using API.Common;

namespace API.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = String.Empty;
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}