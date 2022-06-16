using System;

namespace ToDoBackend.BLL.Models
{
    public class CaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public int ProjectId { get; set; }
    }
}