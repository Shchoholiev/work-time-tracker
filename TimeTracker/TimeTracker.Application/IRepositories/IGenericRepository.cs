﻿using System.Linq.Expressions;
using TimeTracker.Core.Entities;

namespace TimeTracker.Application.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        void Attach(params object[] entities);

        Task<TEntity> GetAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);


        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                               params Expression<Func<TEntity, object>>[] includeProperties);

        Task SaveAsync();
    }
}
