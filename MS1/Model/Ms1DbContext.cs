using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MS1.Model
{
    public class Ms1DbContext : DbContext
    {

      //  public bool UseIntProperty { get; set; }
        public Ms1DbContext(DbContextOptions<Ms1DbContext> options)
      : base(options)
        {


        }
        public virtual DbSet<ModelMS1> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        
        }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //=> optionsBuilder.UseInMemoryDatabase("Ms1DbContext").ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
    }

    //public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    //{


    //    public object Create(DbContext context)
    //        => context is Ms1DbContext dynamicContext
    //            ? (context.GetType(), dynamicContext.UseIntProperty)
    //            : (object)context.GetType();
    //}
}
