namespace UserAuthenticationService.Utils
{
    public class OTPGenerator
    {
        public static string Generate()
        {
            return new Random().NextInt64(100000, 1000000).ToString();
        }
    }
}
