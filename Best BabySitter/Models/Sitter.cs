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
        public string f_name { get; set; }
        public string L_name { get; set; }
        public string AboutMe { get; set; }
        public string contact_NO { get; set; }
        public string email { get; set; }
        public string Location { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string service_Duration { get; set; }
        public Nullable<int> chargePerService { get; set; }
        public string profilePicPath { get; set; }
        public string cv_filePath { get; set; }
        public bool suspended { get; set; }
        public bool verified { get; set; }
        public bool hasProfile { get; set; }
        public string password { get; set; }
    }
}