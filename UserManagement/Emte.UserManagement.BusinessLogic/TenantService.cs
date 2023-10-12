using System;
using AutoMapper;
using Emte.Core.API;
using Emte.Core.Authentication.Contract;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.BusinessLogic.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models.Request;
using Emte.UserManagement.Models.Response;
namespace Emte.UserManagement.BusinessLogic
{
    public class TenantService : ITenantService
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<TenantStatuses> _tenantStatusRepository;
        private readonly IAuthenticationService<AppUser, AppUserTokenMap> _authenticationService;
        private readonly IEntityService<TenantDbContextBase> _tenantEntityService;
        private readonly IEntityService<ClientDBContextBase> _clientEntityService;
        private readonly IRepository<AppRole> _roleRepository;
        private readonly IRepository<AppAccess> _accessRepository;
        private readonly IAppEmailConfig _appEmailConfig;

        public TenantService(
            IEmailService emailService,
            IRepository<Tenant> tenantRepository,
            IRepository<TenantStatuses> tenantStatusRepository,
            IEntityService<TenantDbContextBase> tenantEntityService,
            IEntityService<ClientDBContextBase> clientEntityService,
            IAuthenticationService<AppUser, AppUserTokenMap> authenticationService,
            IRepository<AppRole> roleRepository,
            IRepository<AppAccess> accessRepository,
            IMapper mapper,
            IAppEmailConfig appEmailConfig)
        {
            _mapper = mapper;
            _emailService = emailService;
            _tenantRepository = tenantRepository;
            _tenantStatusRepository = tenantStatusRepository;
            _authenticationService = authenticationService;
            _tenantEntityService = tenantEntityService;
            _clientEntityService = clientEntityService;
            _roleRepository = roleRepository;
            _accessRepository = accessRepository;
            _appEmailConfig = appEmailConfig;
        }

        public async Task<CreateTenantResponse> Subscribe(CreateTenantRequest model, CancellationToken cancellationToken)
        {
            var requestedStatusId = _tenantStatusRepository.Set.Single(r => r.Name == Constants.TenantStaus.Requested).Id;
            var tenant = _mapper.Map<Tenant>(model);

            tenant.Id = Guid.NewGuid();
            tenant.StatusId = requestedStatusId;

            var createdTenant = await _tenantRepository.CreateAsync(tenant, cancellationToken);
            await _tenantEntityService.SaveAsync(cancellationToken);
            await SendAcknowledgement(model, cancellationToken);
            await SendApprovalRequest(tenant, cancellationToken);
            return _mapper.Map<CreateTenantResponse>(tenant);
        }

        public async Task<TenantDataResponse[]> GetAllTenants(CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetAllAsync(cancellationToken);
            return tenants.Select(t => _mapper.Map<TenantDataResponse>(t)).ToArray();
        }

        private async Task SendAcknowledgement(CreateTenantRequest model, CancellationToken cancellationToken)
        {
            var request = new Emte.Core.Models.Request.SendMailRequest
            {
                Body = "You are successfully subscribed, You' ll be notified soon with your credentials",
                Subject = "Subscription Acknowledgement",
                ToAddress = model.Email!
            };
            await _emailService.SendMail(request, cancellationToken);
        }

        private async Task SendApprovalRequest(Tenant tenant, CancellationToken cancellationToken)
        {
            var request = new Emte.Core.Models.Request.SendMailRequest
            {
                Body = $"Please do approve the tenant {tenant.Email} with ID: {tenant.Id}",
                Subject = "Approval Request",
                ToAddress = _appEmailConfig.Email
            };
            await _emailService.SendMail(request, cancellationToken);
        }

        private async Task SendRequestApproved(Tenant tenant, string password, CancellationToken cancellationToken)
        {
            var request = new Emte.Core.Models.Request.SendMailRequest
            {
                Body = $"Your request have been approved and your creds username: {tenant.Email} and password: {password}",
                Subject = "Approval Request",
                ToAddress = $"{tenant.Email}"
            };
            await _emailService.SendMail(request, cancellationToken);
        }

        public async Task<ApproveTenantResponse> ApproveTenant(Guid tenantId, CancellationToken cancellationToken)
        {
            var requestedStatusId = _tenantStatusRepository.Set.Single(r => r.Name == Constants.TenantStaus.Approved).Id;
            var tenant = await _tenantRepository.ReadByIdAsync(tenantId, cancellationToken);
            tenant.StatusId = requestedStatusId;

            await _tenantEntityService.SaveAsync(cancellationToken);
            await _clientEntityService.MigrateAsync(cancellationToken);

            var createUserRequest = _mapper.Map<CreateUserRequest>(tenant);
            createUserRequest.ConfirmPassword = createUserRequest.Password;

            var appUser = await _authenticationService.RegisterUser(createUserRequest, cancellationToken);
            await _clientEntityService.SaveAsync(cancellationToken);
            await SendRequestApproved(tenant, createUserRequest.Password!, cancellationToken);
            return new ApproveTenantResponse { Email = tenant.Email, Password = createUserRequest.Password };
        }

        public async Task Migrate(CancellationToken cancellationToken)
        {
            await _clientEntityService.MigrateAsync(cancellationToken);
        }
    }
}

