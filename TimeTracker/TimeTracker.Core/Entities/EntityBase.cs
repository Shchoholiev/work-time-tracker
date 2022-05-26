using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Core.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
