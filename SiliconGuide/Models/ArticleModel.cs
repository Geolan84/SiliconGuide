using System.ComponentModel.DataAnnotations;

namespace SiliconGuide.Models
{
    public class ArticleModel
    {
        [Required(ErrorMessage = "Текст не может быть пустым")]
        public string Text { get; set; }

        public int ArticleID { get; set; }
    }
}
