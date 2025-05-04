using System.Linq.Expressions;
using TimeTracker.Core.Entities;

namespace TimeTracker.Application.IRepositories
{
    public interface IRecordsRepository
    {
        Task AddAsync(Record record);

        Task UpdateAsync(Record record);

        Task DeleteAsync(Record record);

        Task<Record?> GetAsync(int id);

        Task<IEnumerable<Record>> GetAllAsync(Expression<Func<Record, bool>> predicate);

        Task<int> GetTrackedTimeAsync(int employeeId, DateOnly date);

        Task<int> GetTrackedTimeAsync(int employeeId, int year, int weekOfYear);
    }
}
