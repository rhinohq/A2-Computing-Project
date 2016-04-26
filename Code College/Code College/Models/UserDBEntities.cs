using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Code_College.Models
{
    // Database context for user database
    public class User
    {
        public string Email { get; set; }

        [Key]
        public string Name { get; set; }

        public string PasswordHash { get; set; }
        public int UserLevel { get; set; }
        public string Username { get; set; }
    }

    public class UserDBEntities : DbContext
    {
        public UserDBEntities() : base("name=UserDBEntities")
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}