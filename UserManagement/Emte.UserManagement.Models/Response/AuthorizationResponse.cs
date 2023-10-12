using Emte.Core.Authentication.Contract;

namespace Emte.Core.Authentication.Impl
{
    public class AuthorizationResponse : IAuthorizationResponse
    {
        public AuthorizationResponse(bool isAuthorized)
        {
            IsAuthorized = isAuthorized;
        }
        public bool IsAuthorized { get; set; }
    }
}