using System;

namespace ToDoBackend.DAL.Entities
{
    public class Case : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }

    public enum Priority
    {
        Low,
        Medium,
        Urgent
    }

    public enum Status
    {
        NotStarted,
        InProcess,
        Finished
    }
}