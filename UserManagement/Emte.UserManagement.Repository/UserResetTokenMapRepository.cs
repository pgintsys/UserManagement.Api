using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;

namespace Emte.UserManagement.Repository
{
    public class UserResetTokenMapRepository<T> : RepositoryBase<AppUserResetTokenMap, T>, IRepository<AppUserResetTokenMap>
    where T : ClientDBContextBase
    {
        public UserResetTokenMapRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

