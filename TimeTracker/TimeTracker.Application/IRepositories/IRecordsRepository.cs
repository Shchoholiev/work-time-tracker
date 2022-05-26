using System.Linq.Expressions;
using TimeTracker.Core.Entities;

namespace TimeTracker.Application.IRepositories
{
    public interface IRecordsRepository
    {
        Task AddAsync(Record entity);

        Task UpdateAsync(Record entity);

        Task DeleteAsync(Record entity);

        Task<Record> GetOneAsync(int id);

        Task<IEnumerable<Record>> GetAllAsync(Expression<Func<Record, bool>> predicate);

        Task<int> GetTrackedTime(int employeeId, DateOnly date);

        Task<int> GetTrackedTime(int employeeId, int weekNumber);
    }
}
