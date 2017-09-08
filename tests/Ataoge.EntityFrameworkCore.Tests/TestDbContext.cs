using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class TestDbContext  : AtaogeDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base (options)
        {

        }

        public TestDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=test.db");
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            
             base.OnModelCreating(modelBuilder);
         }
    }

    public class Test
    {
        public int Id {get; set;}
    }
}