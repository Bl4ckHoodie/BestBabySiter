using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Best_BabySitter.Models
{
    public class Slot
    {
        [Key]
        public int ID { get; set; }
        [Display(Name ="Sitter ID")]
        [Required(ErrorMessage = "")]
        public int sitterID { get; set; }
        
        [Display(Name ="Slot Time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Enter slot Time")]
        public DateTime time { get; set; }

        [Display(Name ="Slot Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter slot date")]
        public DateTime date { get; set; }

        [Display(Name ="City")]
        public string city { get; set; }

    }
}