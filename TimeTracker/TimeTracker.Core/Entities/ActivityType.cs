namespace TimeTracker.Core.Entities
{
    public class ActivityType : EntityBase
    {
        public string Name { get; set; }

        public List<Record>? Records { get; set; }
    }
}
