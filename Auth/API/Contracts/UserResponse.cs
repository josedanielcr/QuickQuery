namespace API.Contracts
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
