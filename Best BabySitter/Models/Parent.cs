using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Best_BabySitter.Models
{
    public class Parent
    {
        [Key]
        public int parent_ID { get; set; }

        [Required(ErrorMessage = "Please enter FirstName")]
        [Display(Name = "First Name")]
        public string f_name { get; set; }

        [Required(ErrorMessage = "Please enter LastName")]
        [Display(Name = "Last Name")]
        public string l_name { get; set; }

        [Required(ErrorMessage = "Please enter StreetName")]
        [Display(Name = "Street")]
        public string street { get; set; }

        [Required(ErrorMessage = "Please enter CityName")]
        [Display(Name = "City")]
        public string city { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        [RegularExpression(@"^([a-zA-Z0-9@*#]{8,15})$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase      Alphabet, 1 Number and 1 Special Character")]
        public string password { get; set; }
    }
}