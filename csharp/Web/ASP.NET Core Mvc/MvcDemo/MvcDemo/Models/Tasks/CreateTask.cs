using System.ComponentModel.DataAnnotations;

namespace MvcDemo.WebSite.Models.Tasks
{
    public class CreateTask
    {
        [Required, StringLength(50)]
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

    }
}
