using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.Data;
using Ataoge.Data.Entities;
using Ataoge.EntityFrameworkCore.Repositories;
using Ataoge.Extensions;
using Ataoge.Reflection;
using Ataoge.Repositories;
using Ataoge.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace  Ataoge.EntityFrameworkCore
{
    public class AtaogeDbContext<TKey> :AtaogeDbContext
        where TKey : IEquatable<TKey>
    {
        protected AtaogeDbContext(DbContextOptions options, ISafSession<TKey> safSession)
            : base (options)
        {
            this.SafSession = safSession;
        }

         protected ISafSession<TKey> SafSession {get;}
    }

    public class AtaogeDbContext : DbContext, IRepositoryContext, IRepositoryHelper
    {
        public AtaogeDbContext(DbContextOptions<AtaogeDbContext> options)
            : base(options)
        {
            
        }

        protected AtaogeDbContext(DbContextOptions options)
            : base(options)
        {
            InitializeDbContext();
        }

        ///<summary>
        ///For test only
        ///</summary>
        protected AtaogeDbContext()
        {

        }

       


        private void InitializeDbContext()
        {
            IServiceProvider sp = ((IInfrastructure<IServiceProvider>)(this)).Instance;
        
            var dbContextServices = sp.GetRequiredService<IDbContextServices>();
            InvariantName = dbContextServices.DatabaseProviderServices.InvariantName;
            //ModelManager = sp.GetRequiredService<ModelManager>();
            SqlGenerationHelper = sp.GetRequiredService<ISqlGenerationHelper>();
        }

        protected ISqlGenerationHelper SqlGenerationHelper {get; private set;}

        public string InvariantName {get; private set;}

        public string CreateParameterName(string name)
        {
            return SqlGenerationHelper.GenerateParameterName(name);
        }

        public DbParameter CreateDbParmeter(string name, object value)
        {
            DbCommand dbCommand = Database.GetDbConnection().CreateCommand();
            DbParameter dbParameter =dbCommand.CreateParameter();
            dbParameter.ParameterName = name;
            dbParameter.Value = value;
            return dbParameter;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SequencesRule>( b=>{
                b.HasKey(t => t.PatternName);
                b.Property(t => t.PatternName)
                    .HasColumnName(ConvertName(nameof(SequencesRule.PatternName)));
                b.Property(t => t.MinValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.MinValue)));
                b.Property(t => t.MaxValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.MaxValue)));
                b.Property(t => t.NextValue)
                    .HasColumnName(ConvertName(nameof(SequencesRule.NextValue)));
                b.Property(t => t.Continuum)
                    .HasColumnName(ConvertName(nameof(SequencesRule.Continuum)));
                b.Property(t => t.TableField)
                    .HasColumnName(ConvertName(nameof(SequencesRule.TableField)));
                b.Property(t => t.PreservedCount)
                    .HasColumnName(ConvertName(nameof(SequencesRule.PreservedCount)));
                b.Property(t => t.Step)
                    .HasColumnName(ConvertName(nameof(SequencesRule.Step)));
                b.Property( t => t.UpdateTime)
                    .HasColumnName(ConvertName(nameof(SequencesRule.UpdateTime)))
                    .ValueGeneratedOnAdd()
                    .IsConcurrencyToken();
             
                 b.ToTable(ConvertName(nameof(SequencesRule)));
             });
        }

        public virtual string ConvertName(string fromEntityName)
        {
            switch (InvariantName)
            {
                //case "MySql.Data.EntityFrameworkCore":
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return fromEntityName.ToLower();
                case "Microsoft.EntityFrameworkCore.Sqlite":
                default:
                    return fromEntityName;
            }
        }


        public override int SaveChanges()
        {
            ApplySafConcepts();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplySafConcepts();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected virtual void ApplySafConcepts()
        {
            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //if (!entry.IsKeySet)
                        CheckAndSetId(entry);
                        CheckAndSetMustHaveTenantIdProperty(entry.Entity);
                        CheckAndSetMayHaveTenantIdProperty(entry.Entity);
                        break;
                    case EntityState.Modified:
                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        break;
                }
                TriggerDomainEvents(entry.Entity);
            }
                
            
        }

        protected virtual void CheckAndSetId(EntityEntry entry)
        {
        
           var intEntity = entry.Entity as IEntity<int>;
           if (intEntity!=null) {
                if (intEntity.Id > 0) //already has value
                    return; 
                var dbGeneratedAttr = ReflectionHelper
                        .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                        );

                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    var dbAutoCreateAttr = ReflectionHelper
                        .GetSingleAttributeOrDefault<DbAutoCreatedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                        );
                    if (dbAutoCreateAttr != null)
                    {
                        string patternName =  dbAutoCreateAttr.PatternerName;
                        if (string.IsNullOrEmpty(patternName))
                        {
                            //var oc = this.ModelManager.FindObjectClass(entry.Entity.GetType());
                            //patternName = oc.TableName + "." + oc.FindProperty("Id").Name;
                            patternName = entry.Metadata.Name + "." + entry.Metadata.FindProperty("Id").GetFieldName();
                        }
                    
                        intEntity.Id = NextValue(patternName);
                    

                    }
                }
            }
          

            var strEntity = entry.Entity as IEntity<string>;
            if (strEntity != null)
            {
                var dbGeneratedAttr = ReflectionHelper
                        .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                        );

                if (dbGeneratedAttr == null || dbGeneratedAttr.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    var dbAutoCreateAttr = ReflectionHelper
                        .GetSingleAttributeOrDefault<DbAutoCreatedAttribute>(
                        entry.Property("Id").Metadata.PropertyInfo
                        );
                    if (dbAutoCreateAttr != null)
                    {
                        string patternName =  dbAutoCreateAttr.PatternerName;
                        if (string.IsNullOrEmpty(patternName))
                        {
                            //var oc = this.ModelManager.FindObjectClass(entry.Entity.GetType());
                            //patternName = oc.TableName + "." + oc.FindProperty("Id");
                            patternName = entry.Metadata.Name + "." + entry.Metadata.FindProperty("Id").GetFieldName();
                        }
                    

                        string formatString = dbAutoCreateAttr.Format;
                        if (string.IsNullOrEmpty(formatString))
                            formatString = "{0}";
                        strEntity.Id = string.Format(dbAutoCreateAttr.Format,  NextValue(patternName));
                            

                    }
                }
            }
            
        }

        private int NextValue(string name)
        {
            SequencesRule sr = this.Set<SequencesRule>().SingleOrDefault(t => t.PatternName == name);
            if (sr == null)
            {
                sr = new SequencesRule() {PatternName = name, MinValue = 1, MaxValue = int.MaxValue, NextValue= 2 };
                this.Add(sr);
                return sr.MinValue;
            }
            else
            {
                var value = sr.NextValue;
                sr.NextValue += sr.Step;
                sr.UpdateTime = DateTime.Now;
                this.Update(sr);
                return value;
            }
       


        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.State = EntityState.Unchanged; //TODO: Or Modified? IsDeleted = true makes it modified?
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
        }

        protected virtual void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        {
        //Only set IMustHaveTenant entities
        if (!(entityAsObj is IMustHaveTenant))
        {
            return;
        }

        var entity = entityAsObj.As<IMustHaveTenant>();

        //Don't set if it's already set
        if (entity.TenantId != 0)
        {
            return;
        }
        }



        protected virtual void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        {
            //Only works for single tenant applications
            if (true)//(MultiTenancyConfig.IsEnabled)
            {
                return;
            }

            //Only set IMayHaveTenant entities
            if (!(entityAsObj is IMayHaveTenant))
            {
                return;
            }

            var entity = entityAsObj.As<IMayHaveTenant>();

            //Don't set if it's already set
            if (entity.TenantId != null)
            {
                return;
            }
        }

        protected virtual void TriggerDomainEvents(object entityAsObj)
        {

        }
        
    }
}