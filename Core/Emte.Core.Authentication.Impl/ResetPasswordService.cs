using AutoMapper;
using Emte.Core.Authentication.Contract;
using Emte.Core.DomainModels;
using Emte.Core.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using IDomain = Emte.Core.DomainModels.IDomain;

namespace Emte.Core.Authentication.Impl
{
    public class ResetPasswordService<T, T1> : IResetPasswordService<T, T1>
        where T : class, IDomain, ITokenDomain<T1>, new()
        where T1 : class, IDomain, IUserDomain
    {
        private readonly IAuthenticationService<T1, T> _authenticationService;
        private readonly IUserRepository<T1> _userRepository;
        private readonly IRepository<T> _resetUserTokenRepository;
        private readonly IMapper _mapper;

        public ResetPasswordService(
            IUserRepository<T1> userRepository,
            IRepository<T> resetUserTokenRepository,
            IMapper mapper,
            IAuthenticationService<T1, T> authenticationService)
        {
            _userRepository = userRepository;
            _resetUserTokenRepository = resetUserTokenRepository;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }
        public async Task<bool> CanResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var token = await _resetUserTokenRepository.Set.SingleOrDefaultAsync(r => r.Token == request.ResetToken, cancellationToken);
            return token != null;
        }

        public async Task InitiateResetPassword(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Set.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user == null) { throw new Exception("User not found"); }
            var tokenMap = _mapper.Map<T>(user);
            var token = await _resetUserTokenRepository.CreateAsync(tokenMap, cancellationToken);
            Console.WriteLine($"reset token {token.Entity.Token}");
        }

        public async Task ResetPassword(IResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var token = await _resetUserTokenRepository.Set.SingleOrDefaultAsync(r => r.Token == request.ResetToken, cancellationToken);
            if (token == null)
            {
                throw new Exception("Invalid Token");
            }

            await _authenticationService.ResetPassword(request, cancellationToken);
            await _resetUserTokenRepository.DeleteByIds<T>(new[] { token.Id }, cancellationToken);
        }
    }
}