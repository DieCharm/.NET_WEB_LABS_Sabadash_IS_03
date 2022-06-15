using System.Collections.Generic;

namespace ToDoBackend.DAL.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Case> Tasks { get; set; }
    }
}