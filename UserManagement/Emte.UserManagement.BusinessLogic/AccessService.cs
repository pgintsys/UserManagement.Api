using System;
using AutoMapper;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.BusinessLogic.Contracts;
using Emte.UserManagement.DomainModels;
using Emte.UserManagement.Models.Request;

namespace Emte.UserManagement.BusinessLogic
{
	public class AccessService : IAccessService
	{
        private readonly IMapper _mapper;
        private IRepository<AppAccess> _accessRepository;

        public AccessService(
            IMapper mapper,
            IRepository<AppAccess> accessRepository)
        {
            _mapper = mapper;
            _accessRepository = accessRepository;
        }

        public async Task CreateAccesses(CreateAccessRequest[] accessRequests, CancellationToken cancellationToken)
        {
            var rolesToCreate = accessRequests.Select(r => _mapper.Map<AppAccess>(r)).ToArray();
            await _accessRepository.CreateMultipleAsync(rolesToCreate, cancellationToken);
        }
    }
}

