namespace api.Dto.Account
{
    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int? RoleId { get; set; }

    }
}
