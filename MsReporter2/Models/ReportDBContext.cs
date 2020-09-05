using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsReporter3.Models
{
    public partial class ReportDBContext    :DbContext
    {
        public ReportDBContext(DbContextOptions<ReportDBContext> options): base(options)
        {


        }
        public virtual DbSet<ReportModels> ReportHello { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
