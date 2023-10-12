namespace Emte.Core.Authentication.Contract
{
    public interface IAuthorizationResponse
    {
        bool IsAuthorized { get; set; }
    }
}