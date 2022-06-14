using System.ComponentModel.DataAnnotations;

namespace MvcDemo.WebSite.Models
{
    public class PostModel
    {
        [Required]
        public string? Title { get; set; }

        public string? Body { get; set; }

    }
}
