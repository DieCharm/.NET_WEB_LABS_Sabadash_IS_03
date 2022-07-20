namespace ToDoBackend.BLL.Models
{
    public class ProjectUserModel
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
