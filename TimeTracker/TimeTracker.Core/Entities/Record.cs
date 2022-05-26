namespace TimeTracker.Core.Entities
{
    public class Record : EntityBase
    {
        public int HoursWorked { get; set; }

        public DateTime Date { get; set; }

        public Employee Employee { get; set; }

        public Role Role { get; set; }

        public ActivityType ActivityType { get; set; }

        public Project Project { get; set; }
    }
}
