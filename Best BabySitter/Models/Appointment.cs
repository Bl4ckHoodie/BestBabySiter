using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Best_BabySitter.Models
{
    public class Appointment
    {
        [Display(Name = "Appointee")]
        [Required(ErrorMessage = "Enter appointee name")]
        public string appointee { get; set; }
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter start date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter end date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Enter start time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Enter end time")]
        public DateTime EndTime { get; set; }
        [Required(ErrorMessage = "Enter street address")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Enter city")]
        public string City { get; set; }

        public string email { get; set; }
    }
}