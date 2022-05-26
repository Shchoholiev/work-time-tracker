namespace TimeTracker.Application.DTO
{
    public class EmployeeDTO : EntityBaseDTO
    {
        public string Name { get; set; }

        public SexDTO Sex { get; set; }

        public DateTime Birthday { get; set; }
    }
}
