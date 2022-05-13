using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace SiliconGuide.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        string connectionString = null;
        public CategoryRepository(string conn)
        {
            connectionString = conn;
        }

        Category? ICategoryRepository.GetCategory(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    return db.Query<Category>("SELECT * FROM Categories WHERE ArticleCategoryID=@id", new { id }).FirstOrDefault();
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }

        string? ICategoryRepository.GetText(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    return db.Query<string>("SELECT Text FROM Articles WHERE ArticleID=@id", new { id }).FirstOrDefault();
                }
            }
            catch (SqlException)
            {
                return String.Empty;
            }

        }

        public List<Category> GetCategories()
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var result = db.Query<Category>("GetTree", commandType: CommandType.StoredProcedure).ToList();
                    return result;
                }
            }
            catch (SqlException)
            {
                return null;
            }

        }

        public List<Category> FindWord(string search)
        {
            try
            {
                search = search.Substring(0, search.Length > 49 ? 49 : search.Length);
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var res = db.Query<Category>("FindWord", new { word = $"%{search}%" }, commandType: CommandType.StoredProcedure).ToList();
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public void Delete(int id)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    var category = db.Query<Category>("SELECT * FROM Categories WHERE ArticleCategoryID=@id", new { id }).FirstOrDefault();
                    var sqlQueryCategory = "DELETE FROM Categories WHERE ArticleCategoryID = @id";
                    db.Execute(sqlQueryCategory, new { id });
                }
            }
            catch (SqlException)
            {

            }
        }

        public int GetFreeCategoryID()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var id = db.Query<int>("SELECT MAX(ArticleCategoryID) FROM Categories").FirstOrDefault();
                return id + 1;
            }
        }

        public int GetFreeArticleID()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var id = db.Query<int>("SELECT MAX(ArticleID) FROM Articles").FirstOrDefault();
                return id + 1;
            }
        }

        public void CreateCategory(Category category)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = "Insert Into Categories (ArticleCategoryID, ParentCategoryID, Name, Priority, ArticleID) Values(@ArticleCategoryID, @ParentCategoryID, @Name, @Priority, @ArticleID)";
                    int rowsAffected = db.Execute(sqlQuery, category);
                }
            }
            catch (SqlException)
            {

            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE Categories SET ParentCategoryID=@ParentCategoryID, Name=@Name, Priority=@Priority, ArticleID=@ArticleID  WHERE ArticleCategoryID = @ArticleCategoryID";
                    var result = db.Execute(updateQuery, category);
                }
            }
            catch (SqlException)
            {

            }
        }

        void ICategoryRepository.CreateArticle(ArticleModel model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = "Insert Into Articles (ArticleID, Text) Values(@ArticleID, @Text)";
                    int rowsAffected = db.Execute(sqlQuery, model);
                }
            }
            catch (SqlException)
            {

            }

        }

        void ICategoryRepository.UpdateArticle(ArticleModel model)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE Articles SET Text=@Text WHERE ArticleID = @ArticleID";
                    var result = db.Execute(updateQuery, model);
                }
            }
            catch (SqlException)
            {

            }

        }

        ArticleModel? ICategoryRepository.GetArticle(int ArticleID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    return db.Query<ArticleModel>("SELECT * FROM Articles WHERE ArticleID=@ArticleID", new { ArticleID }).FirstOrDefault();
                }
            }
            catch (SqlException)
            {
                return null;
            }

        }
    }
}
