using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeTracker.Application.IRepositories;
using TimeTracker.Application.Paging;
using TimeTracker.Core.Entities;
using TimeTracker.Infrastructure.EF;

namespace TimeTracker.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<TEntity> _table;

        public GenericRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity item)
        {
            await this._table.AddAsync(item);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(TEntity item)
        {
            this._table.Update(item);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(TEntity item)
        {
            this._table.Remove(item);
            await this.SaveAsync();
        }

        public void Attach(params object[] entities)
        {
            this._db.AttachRange(entities);
        }

        public async Task<TEntity?> GetAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity?> GetAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entity = this.Include(_table.Where(e => e.Id == id), includeProperties);
            return await entity.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking();
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                                     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking().Where(predicate);
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters)
        {
            var entities = await this._table
                                     .AsNoTracking()
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate(query, (current, includeProperty)
                                                => current.Include(includeProperty));
        }

        public async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
