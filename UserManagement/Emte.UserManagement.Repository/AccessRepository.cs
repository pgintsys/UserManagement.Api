using System;
using Emte.Core.DataAccess;
using Emte.Core.Repository.Contracts;
using Emte.UserManagement.DataAccess;
using Emte.UserManagement.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Emte.UserManagement.Repository
{
	public class AccessRepository<T> : RepositoryBase<AppAccess, T>, IRepository<AppAccess>
    where T : ClientDBContextBase
    {
        public AccessRepository(IQueryableConnector<T> queryableConnector) : base(queryableConnector)
        {
        }
    }
}

