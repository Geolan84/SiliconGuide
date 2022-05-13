using System.ComponentModel.DataAnnotations;

namespace SiliconGuide.Models
{
    public class User
    {
        public string Login { get; set; }
        public int UserID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string? Organisation { get; set; }
        public byte AccessLevel { get; set; }
    }
}
