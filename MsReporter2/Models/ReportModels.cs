using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsReporter3.Models
{
    public class ReportModels
    {
        public ReportModels()
        {

        }

        [Key]
        [Required]
        [Column(TypeName = "decimal(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Required]
        public string message { get; set; }
        //public int Counter { get; set; }
    }
}
