using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconGuide.Models
{
    public interface ICategoryRepository
    {
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        Category? GetCategory(int categoryID);
        public List<Category> FindWord(string search);
        void Delete(int id);
        string? GetText(int id);
        List<Category> GetCategories();
        int GetFreeCategoryID();
        int GetFreeArticleID();
        void CreateArticle(ArticleModel model);    
        void UpdateArticle(ArticleModel model);
        ArticleModel? GetArticle(int ArticleID);
    }
}
