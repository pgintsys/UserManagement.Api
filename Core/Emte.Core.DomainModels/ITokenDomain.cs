using System;
namespace Emte.Core.DomainModels
{
    public interface ITokenDomain<T> : IWithId
    {
        Guid UserId { get; set; }
        T? AppUser { get; set; }
        string? Token { get; set; }
    }
}

