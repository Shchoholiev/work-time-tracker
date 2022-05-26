namespace TimeTracker.Core.Entities
{
    public class Employee : EntityBase
    {
        public string Name { get; set; }

        public string Sex { get; set; }

        public DateTime Birthday { get; set; }
    }
}
