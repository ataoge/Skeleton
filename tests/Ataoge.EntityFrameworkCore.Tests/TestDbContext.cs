using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class TestDbContext  : AtaogeDbContext
    {
        public TestDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=test.db");
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);
         }
    }
}