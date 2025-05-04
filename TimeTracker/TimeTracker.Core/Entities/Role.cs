namespace TimeTracker.Core.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public List<Record>? Records { get; set; }
    }
}
