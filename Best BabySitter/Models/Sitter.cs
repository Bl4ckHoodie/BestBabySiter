using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Best_BabySitter.Models
{
    public class Sitter
    {
        [Key]
        public int sitter_ID { get; set; }
        [Display(Name = "First Name")]
        public string f_name { get; set; }
        [Display(Name = "Last Name")]
        public string L_name { get; set; }
        [Display(Name = "About Me")]
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }
        [Display(Name = "Contact Number")]
        public string contact_NO { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [Display(Name = "Email Address")]
        public string email { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Street")]
        public string street { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "Service Duration")]
        public string service_Duration { get; set; }
        public Nullable<int> chargePerService { get; set; }
        public string profilePicPath { get; set; }
        [Display(Name = "Upload CV")]
        public string cv_filePath { get; set; }
        public bool suspended { get; set; }
        public bool verified { get; set; }
        public bool hasProfile { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        public string password { get; set; }
    }
}