using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.Repository
{
    public class AppUserRoleMapRepository<T> : RepositoryBase<AppUserRoleMap, T>, IRepository<AppUserRoleMap>
    where T : ClientDBContextBase
    {
        public AppUserRoleMapRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

