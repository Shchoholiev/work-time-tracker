namespace TimeTracker.Application.DTO.Create
{
    public class RecordCreateDTO
    {
        public int HoursWorked { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public EmployeeDTO Employee { get; set; }

        public RoleDTO Role { get; set; }

        public ActivityTypeDTO ActivityType { get; set; }

        public ProjectDTO Project { get; set; }
    }
}
