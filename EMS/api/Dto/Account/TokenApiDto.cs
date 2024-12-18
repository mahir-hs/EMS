namespace api.Dto.Identity
{
    public class TokenApiDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; } 
    }
}
