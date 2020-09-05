using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS2.Model
{
    public partial class ModelMS2
    {
        public ModelMS2()
        {

        }

        [Key]
        [Required]
        [Column(TypeName = "decimal(18,0)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Required]
        public string message2 { get; set; }
        //public int Counter { get; set; }

    }
}
