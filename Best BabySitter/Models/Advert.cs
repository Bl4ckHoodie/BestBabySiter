using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Best_BabySitter.Models
{
    public class Advert
    {
        [Key]
        public int ID { get; set; }
        [Display(Name="Number of kids to be caredfore")]
        [Required(ErrorMessage ="Enter number of kids")]

        public int NumKids { get; set; }
        [Display(Name = "Age Range")]
        [Required(ErrorMessage = "Enter age range e.g 7-15")]
        public string AgeRange { get; set; }
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

        [DataType(DataType.MultilineText)]
        [Display(Name = "Specific Requirements")]
        public string Specification { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }

 
}