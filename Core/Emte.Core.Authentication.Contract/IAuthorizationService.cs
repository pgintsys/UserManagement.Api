public interface IAuthorizationService {
    Task<bool> IsAuthorized(string access, Guid userId, CancellationToken cancellationToken);
}