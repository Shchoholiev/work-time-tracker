namespace TimeTracker.Core.Entities
{
    public class Employee : EntityBase
    {
        public string Name { get; set; }

        public Sex Sex { get; set; }

        public DateTime Birthday { get; set; }

        public List<Record>? Records { get; set; }
    }
}
