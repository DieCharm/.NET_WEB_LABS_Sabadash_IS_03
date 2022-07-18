namespace ToDoBackend.BLL.Interfaces
{
    public interface IMailService
    {
        public bool SendAsync(string to, string name, string text);
    }
}