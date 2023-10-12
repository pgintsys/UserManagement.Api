using System;
using Emte.UserManagement.Models.Request;

namespace Emte.UserManagement.BusinessLogic
{
	public interface IAccessService
	{
        Task CreateAccesses(CreateAccessRequest[] accessRequests, CancellationToken cancellationToken);

    }
}

