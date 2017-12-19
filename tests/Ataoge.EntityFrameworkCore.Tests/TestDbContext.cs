using System.Collections.Generic;
using Ataoge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

    public class TestEntity : CommonDataEntity<int>
    {
        [JsonExtensionData]
        private IDictionary<string, object> overrideDcits = new Dictionary<string, object>();

        public override object this[string key]
        {
            get 
            {
                if (overrideDcits.ContainsKey(key))
                    return overrideDcits[key];
                return null;
            }
            set
            {
                overrideDcits[key] = value;
            }
        }
    }

    public class Test
    {
        public int Id {get; set;}
    }

    public class TestA 
    {
        public string Name {get; set;}
        public virtual ICollection<Test> Test {get; set;}

        public Test TestClass {get; set;}
    }
}