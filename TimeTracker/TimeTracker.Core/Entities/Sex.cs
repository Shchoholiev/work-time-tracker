namespace TimeTracker.Core.Entities
{
    public class Sex : EntityBase
    {
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
