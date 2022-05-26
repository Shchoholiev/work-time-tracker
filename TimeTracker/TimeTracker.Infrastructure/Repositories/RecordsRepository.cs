using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;
using TimeTracker.Infrastructure.EF;

namespace TimeTracker.Infrastructure.Repositories
{
    public class RecordsRepository : IRecordsRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Record> _table;

        public RecordsRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<Record>();
        }

        public async Task AddAsync(Record record)
        {
            this._db.Attach(record);
            await this._table.AddAsync(record);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(Record record)
        {
            this._db.Attach(record);
            this._table.Update(record);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(Record record)
        {
            this._table.Remove(record);
            await this.SaveAsync();
        }

        public async Task<Record?> GetAsync(int id)
        {
            return await this._table
                             .Include(r => r.Project)
                             .Include(r => r.Employee)
                             .Include(r => r.ActivityType)
                             .Include(r => r.Role)
                             .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Record>> GetAllAsync(Expression<Func<Record, bool>> predicate)
        {
            return await this._table.AsNoTracking()
                                    .Where(predicate)
                                    .Include(r => r.Project)
                                    .Include(r => r.Employee)
                                    .Include(r => r.ActivityType)
                                    .Include(r => r.Role)
                                    .ToListAsync();
        }

        public async Task<int> GetTrackedTime(int employeeId, DateOnly date)
        {
            return await this._table.AsNoTracking()
                                    .Where(r => r.Employee.Id == employeeId 
                                          && r.Date.Year == date.Year
                                          && r.Date.Month == date.Month
                                          && r.Date.DayOfYear == date.DayOfYear)
                                    .SumAsync(r => r.HoursWorked);
        }

        public async Task<int> GetTrackedTime(int employeeId, int year, int weekOfYear)
        {
            var start = this.GetFirstMondayNumber(year);
            var end = 0;

            if (weekOfYear == 1)
            {
                end = start - 1;
                start = 1;
            }
            else
            {
                start += 7 * (weekOfYear - 2);
                end = start + 6;
            }

            return await this._table.AsNoTracking()
                                    .Where(r => r.Employee.Id == employeeId
                                           && r.Date.Year == year
                                           && r.Date.DayOfYear >= start && r.Date.DayOfYear <= end)
                                    .SumAsync(r => r.HoursWorked);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }

        private int GetFirstMondayNumber(int year)
        {
            var date = new DateOnly(year, 1, 1);
            var mondayNumber = 1;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(1);
                mondayNumber++;
            }

            return mondayNumber;
        }
    }
}
