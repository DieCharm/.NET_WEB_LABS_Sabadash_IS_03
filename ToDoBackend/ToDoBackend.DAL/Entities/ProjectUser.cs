namespace ToDoBackend.DAL.Entities
{
    public class ProjectUser : BaseEntity
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public bool IsAdmin { get; set; }
    }
}
