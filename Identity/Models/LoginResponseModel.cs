namespace Identity.Models
{
    public class LoginResponseModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
        public bool EmailVerified { get; set; }
        public IList<string> Roles { get; set; }
    }
}
