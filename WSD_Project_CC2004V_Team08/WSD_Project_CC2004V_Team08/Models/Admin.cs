using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSD_Project_CC2004V_Team08.Models
{
    public class Admin
    {
        public int ID { get; set; }
        [Required]
        public string password { get; set; }
    }
}
