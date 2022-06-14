using System.ComponentModel.DataAnnotations;

namespace MvcDemo.WebSite.Models.Tasks;

public class CompleteTask
{
    [Required]
    public int TaskId { get; set; }

    public string Description { get; set; } = null!;
}