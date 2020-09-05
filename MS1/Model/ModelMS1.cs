using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MS1.Model
{
    public partial class ModelMS1
    {
        public ModelMS1() 
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
