using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Emte.UserManagement.Repository
{
	public class RoleRepository<T> : RepositoryBase<AppRole, T>, IRepository<AppRole>
    where T : ClientDBContextBase
    {
        public RoleRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

