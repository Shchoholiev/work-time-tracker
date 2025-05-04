using TimeTracker.Core.Entities;
using TimeTracker.Infrastructure.EF;

namespace TimeTracker.Infrastructure.DataInitialization
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationContext context)
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var male = new Sex { Name = "Male" };
            var female = new Sex { Name = "Female" };
            await context.Sexes.AddRangeAsync(male, female);
            await context.SaveChangesAsync();

            var petro = new Employee
            {
                Name = "Nabatov Petro",
                Sex = male,
                Birthday = new DateTime(1997, 5, 15),
            };

            var katya = new Employee
            {
                Name = "Shevchenko Kateryna",
                Sex = female,
                Birthday = new DateTime(2001, 5, 15),
            };

            await context.Employees.AddRangeAsync(petro, katya);
            await context.SaveChangesAsync();

            var regularWork = new ActivityType { Name = "Regular Work" };
            var overtime = new ActivityType { Name = "Overtime" };
            await context.ActivityTypes.AddRangeAsync(regularWork, overtime);
            await context.SaveChangesAsync();

            var engineer = new Role { Name = "Software Engineer" };
            var architect = new Role { Name = "Software Architect" };
            var qa = new Role { Name = "QA Tester" };
            var designer = new Role { Name = "Designer" };
            await context.Roles.AddRangeAsync(engineer, architect, qa, designer);
            await context.SaveChangesAsync();

            var bank = new Project
            {
                Name = "Online Banking App",
                StartDate = new DateTime(2022, 3, 10),
                EndDate = new DateTime(2022, 8, 10),
            };

            var education = new Project
            {
                Name = "Web app for Online Education",
                StartDate = new DateTime(2021, 11, 5),
                EndDate = new DateTime(2022, 6, 27),
            };

            await context.Projects.AddRangeAsync(bank, education);
            await context.SaveChangesAsync();

            #region Records

            var record1 = new Record
            {
                HoursWorked = 8,
                Date = new DateTime(2022, 5, 23),
                Role = engineer,
                ActivityType = regularWork,
                Project = bank,
                Employee = petro,
            };

            var record2 = new Record
            {
                HoursWorked = 1,
                Date = new DateTime(2022, 5, 23),
                Role = architect,
                ActivityType = overtime,
                Project = bank,
                Employee = petro,
            };

            var record3 = new Record
            {
                HoursWorked = 6,
                Date = new DateTime(2022, 5, 24),
                Role = engineer,
                ActivityType = regularWork,
                Project = bank,
                Employee = petro,
            };

            var record4 = new Record
            {
                HoursWorked = 8,
                Date = new DateTime(2022, 5, 25),
                Role = engineer,
                ActivityType = regularWork,
                Project = bank,
                Employee = petro,
            };

            var record5 = new Record
            {
                HoursWorked = 7,
                Date = new DateTime(2022, 5, 18),
                Role = qa,
                ActivityType = regularWork,
                Project = bank,
                Employee = katya,
            };

            var record6 = new Record
            {
                HoursWorked = 4,
                Date = new DateTime(2022, 5, 19),
                Role = qa,
                ActivityType = regularWork,
                Project = education,
                Employee = katya,
            };

            var record7 = new Record
            {
                HoursWorked = 8,
                Date = new DateTime(2022, 5, 23),
                Role = qa,
                ActivityType = regularWork,
                Project = education,
                Employee = katya,
            };

            var record8 = new Record
            {
                HoursWorked = 5,
                Date = new DateTime(2022, 5, 24),
                Role = designer,
                ActivityType = regularWork,
                Project = education,
                Employee = katya,
            };

            await context.Records.AddRangeAsync(record1, record2, record3, record4, record5, 
                                                record6, record7, record8);
            await context.SaveChangesAsync();

            #endregion
        }
    }
}
