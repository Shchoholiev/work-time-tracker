namespace TimeTracker.Application.DTO
{
    public class RecordDTO : EntityBaseDTO
    {
        public int HoursWorked { get; set; }

        public EmployeeDTO Employee { get; set; }

        public RoleDTO Role { get; set; }

        public ActivityTypeDTO ActivityType { get; set; }

        public ProjectDTO Project { get; set; }
    }
}
