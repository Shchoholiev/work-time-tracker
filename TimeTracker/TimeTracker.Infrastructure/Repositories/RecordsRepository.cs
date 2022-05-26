using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;
using TimeTracker.Infrastructure.EF;
using System.Globalization;

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

        public async Task<Record?> GetOneAsync(int id)
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
                                          && r.Date.ToShortDateString() == date.ToShortDateString())
                                    .SumAsync(r => r.HoursWorked);
        }

        public async Task<int> GetTrackedTime(int employeeId, int weekNumber)
        {
            
            return await this._table.AsNoTracking()
                                    .Where(r => r.Employee.Id == employeeId
                                           && this.GetWeekOfYear(r.Date) == weekNumber)
                                    .SumAsync(r => r.HoursWorked);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }

        private int GetWeekOfYear(DateTime date)
        {
            var culture = new CultureInfo("uk-UA");
            var calendar = culture.Calendar;
            return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
        }
    }
}
