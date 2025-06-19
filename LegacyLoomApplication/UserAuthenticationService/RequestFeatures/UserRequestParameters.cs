using RequestFeatureShared;

namespace UserAuthenticationService.RequestFeatures
{
    public class UserRequestParameters: RequestParameters
    {
        public UserRequestParameters()
        {
            OrderBy = "username";
        }

        public string? Name { get; set; }
    }
}
