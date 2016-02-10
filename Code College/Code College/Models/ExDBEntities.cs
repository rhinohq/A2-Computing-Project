using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Xml;

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
        public int ExID { get; set; }
        public string ExTitle { get; set; }
        public string ExDescription { get; set; }
        public string ExCodeTemplate { get; set; }
        public XmlDocument ExMarkScheme { get; set; }
    }
}