using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsReporter.Model
{
    public class MsrDbContext : DbContext
    {
        public MsrDbContext(DbContextOptions<MsrDbContext> options)
        : base(options)
        {


        }
        public virtual DbSet<MsrModel> ReportsHello { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
