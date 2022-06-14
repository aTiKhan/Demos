namespace MvcDemo.App.TodoItems;

/// <summary>
///     Something that must be completed.
/// </summary>
public class TodoTask
{
    public TodoTask(string title, string description)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreatedAtUtc = DateTime.UtcNow;
    }

    public string Title { get; }
    public string Description { get; }

    public int UserId { get; set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? CompletedAtUtc { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    public int Id { get; private set; }

    public void Complete()
    {
        CompletedAtUtc = DateTime.UtcNow;
        IsCompleted = true;
    }
}