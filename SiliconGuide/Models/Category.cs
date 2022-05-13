namespace SiliconGuide.Models
{
    public class Category
    {
        public int ArticleCategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string? Name { get; set; }
        public int Priority { get; set; }
        public int? ArticleID { get; set; }
        public int Tree { get; set; }

        public Category()
        {

        }
    }
}
