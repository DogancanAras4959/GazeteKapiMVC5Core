using CORE.ApplicationCore.Repository;
using DOMAIN.DataAccessLayer.Models.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CORE.ApplicationCore.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        Task<int> Commit();
        void RollBack();

    }
}
