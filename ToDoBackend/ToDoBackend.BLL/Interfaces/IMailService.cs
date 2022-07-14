namespace ToDoBackend.BLL.Interfaces
{
    public interface IMailService
    {
        public bool SendAsync(string message);
    }
}