namespace UserAuthenticationService.DTOs.UserAuthenticationDTOs
{
    public class UserLoginResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
