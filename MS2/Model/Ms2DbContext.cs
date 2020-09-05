using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MS2.Model
{
    public class Ms2DbContext : DbContext
    {

      //    public bool UseIntProperty { get; set; }
        public Ms2DbContext(DbContextOptions<Ms2DbContext> options)
      : base(options)
        {


        }
        public virtual DbSet<ModelMS2> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseInMemoryDatabase("Ms2DbContext").ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
    }

    //public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    //{
    //                public object Create(DbContext context)
    //        => context is Ms2DbContext dynamicContext
    //            ? (context.GetType(), dynamicContext.UseIntProperty)
    //            : (object)context.GetType();
    //}
}
