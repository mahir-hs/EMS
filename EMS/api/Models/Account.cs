namespace api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
