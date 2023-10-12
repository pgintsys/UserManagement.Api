using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.Repository
{
    public class UserTokenMapRepository<T> : RepositoryBase<AppUserTokenMap, T>, IRepository<AppUserTokenMap>
    where T : ClientDBContextBase
    {
        public UserTokenMapRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

