using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.Repository
{
    public class AppRoleAccessMapRepository<T> : RepositoryBase<AppRoleAccessMap, T>, IRepository<AppRoleAccessMap>
    where T : ClientDBContextBase
    {
        public AppRoleAccessMapRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

