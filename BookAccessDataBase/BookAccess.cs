using BookDomin;
using Microsoft.EntityFrameworkCore;

namespace BookAccessDataBase
{
    public class BookAccess:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Books;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<PrintsBook> Prints { get; set; }
    }
}