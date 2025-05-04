namespace TimeTracker.Core.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Record>? Records { get; set; }
    }
}
