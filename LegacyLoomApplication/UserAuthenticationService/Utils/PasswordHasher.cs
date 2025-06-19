namespace UserAuthenticationService.Utils
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(hash))
            {
                throw new ArgumentNullException(nameof(hash), "Password hash cannot be null or empty.");
            }

            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
