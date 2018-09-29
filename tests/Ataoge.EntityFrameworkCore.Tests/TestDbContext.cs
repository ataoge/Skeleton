using System.Collections.Generic;
using Ataoge.Data;
using Ataoge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class TestDbContext  : AtaogeDbContext
    {
        //public DbSet<ResourcePermissionAssign> ResourcePermissionAssign {get; set;}
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

             var eb = modelBuilder.Entity<Test>();
             eb.HasKey( t => t.Id);
             eb.Property(t => t.Id);
             eb.Property(t => t.Name);
             eb.ToTable(nameof(Test));

             eb.HasMany(t => t.ResourcePermissionAssigns).WithOne().HasForeignKey(t => t.ResourceId);
            // eb.ToTable("${Test}");
         }


    }

   public class ResourcePermissionAssign :  IEntity<int>
   {
       
        public int Id {get; set;}

    
        public int ResourceId {get;set;}

        public int RoleId {get; set;}

        public string ResourceType {get; set;}

        public int Operation {get; set;}

        public int IsRefused {get; set;}

       // public BasePermissionAssign Base {get; set;}

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

        public string Name {get; set;}

        public virtual ICollection<ResourcePermissionAssign> ResourcePermissionAssigns {get; set;}
    }

    public class TestA 
    {
        public string Name {get; set;}
        public virtual ICollection<Test> Test {get; set;}

        public Test TestClass {get; set;}
    }
}