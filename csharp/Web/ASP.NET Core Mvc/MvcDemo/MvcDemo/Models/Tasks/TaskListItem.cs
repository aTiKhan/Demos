namespace MvcDemo.WebSite.Models.Tasks;

public class TaskListItem
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }
    public bool IsCompleted { get; set; }
    public int DaysActive { get; set; }
}