namespace MvcDemo.App.TodoItems;

public interface ITaskRepository
{
    Task Create(TodoTask task);
    Task Update(TodoTask task);
    Task Delete(TodoTask task);

    Task<TodoTask> Get(int id);

    Task<IReadOnlyList<TodoTask>> List(int userId);
}