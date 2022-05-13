namespace SiliconGuide.Models
{
    public interface IUsersRepository
    {
        User? Get(string email,string password);
        void Create(User user);
        int GetFreeID();
    }
}
