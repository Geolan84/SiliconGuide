using System.Data.SqlClient;
using System.Data;
using Dapper;


namespace SiliconGuide.Models
{
    public class UsersRepository : IUsersRepository
    {
        readonly string? connectionString = null;
        public UsersRepository(string conn)
        {
            connectionString = conn;
        }

        public void Create(User user)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    string sqlQuery = "Insert Into users (Login,UserID,Password,Name,Surname,Patronymic,Email,Organisation,AccessLevel) Values(@Login,@UserID,@Password,@Name,@Surname,@Patronymic,@Email,@Organisation,@AccessLevel)";
                    int rowsAffected = db.Execute(sqlQuery, user);
                }
            }
            catch (SqlException)
            {

            }
        }

        public User? Get(string email, string password)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(connectionString))
                {
                    password = HashOperations.Encode(password, email);
                    var user = db.Query<User>("SELECT * FROM users WHERE Email=@email AND Password=@password", new { email, password }).FirstOrDefault();
                    return user;
                }
            }
            catch (SqlException)
            {
                return null;
            }
        }

        public int GetFreeID()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var id = db.Query<int>("SELECT MAX(UserID) FROM users").FirstOrDefault();
                return id + 1;
            }
        }
    }
}
