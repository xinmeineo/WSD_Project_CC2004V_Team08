using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSD_Project_CC2004V_Team08.Models
{
    public class Order
    {
        public int ID { get; set; }
        [Required]

        public string food_desc { get; set; }

        public string delivery_address { get; set; }
        
        public DateTime delivery_date_time { get; set ;}

        public int quantity { get; set; }

        public DateTime order_date { get; set; }

        public int customer_name { get; set; }

    }
}
