using Microsoft.EntityFrameworkCore;
using Task1.Models;

namespace Task1.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions  options) : base(options)
        {

        }


        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
