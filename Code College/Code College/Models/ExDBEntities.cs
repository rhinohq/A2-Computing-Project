using Marker;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Code_College.Models
{
    public class ExDBEntities : DbContext
    {
        public ExDBEntities() : base("name=ExDBEntities")
        {
        }

        public virtual DbSet<Exercise> Exercises { get; set; }
    }

    public class Exercise
    {
        [Key]
        public int DBID { get; set; }

        public string ExAppendCode { get; set; }

        public string ExCodeTemplate { get; set; }

        [Required]
        public string ExDescription { get; set; }

        [Required]
        public int ExID { get; set; }

        public ExMarkScheme ExMarkScheme { get; set; }

        [Required]
        public string ExTitle { get; set; }
    }
}