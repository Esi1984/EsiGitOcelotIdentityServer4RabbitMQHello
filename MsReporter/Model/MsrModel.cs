using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsReporter.Model
{
    public partial class MsrModel
    {
        public MsrModel()
        {
                 
        }
        [Key]
        [Required]
        [Column(TypeName = "decimal(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }
        public decimal CounterMs1 { get; set; }
        public decimal CounterMs2 { get; set; }
    }
}
