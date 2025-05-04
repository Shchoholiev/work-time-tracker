namespace TimeTracker.Application.DTO
{
    public class ProjectDTO : EntityBaseDTO
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
