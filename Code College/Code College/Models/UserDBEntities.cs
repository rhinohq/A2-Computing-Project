using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Code_College.Models
{
    public class UserDBEntities : DbContext
    {
        public UserDBEntities() : base("name=UserDBEntities")
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }

    public class User
    {
        [Key]
        public string Name { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int UserLevel { get; set; }
    }
}