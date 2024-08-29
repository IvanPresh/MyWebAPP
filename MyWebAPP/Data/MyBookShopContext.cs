using Microsoft.EntityFrameworkCore;
using MyWebAPP.Entities;

namespace MyWebAPP.Data
{
    public class MyBookShopContext:DbContext
    {

        public MyBookShopContext(DbContextOptions<MyBookShopContext> options) : base(options)
        {
        }
        public DbSet<Book> books { get; set; }
    }
}
