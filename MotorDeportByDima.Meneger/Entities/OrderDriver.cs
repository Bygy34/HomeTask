using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Entities
{
    public class OrderDriver
    {
        [Key]
        public string Id { get; set; }

        public string StartAddressCity { get; set; }

        public string StartAddressRoad { get; set; }

        public string StartAddressHouse { get; set; }

        //public string StartAddress { get; set; }

        public string CarBrand { get; set; }

        public int MaxWeight { get; set; }

        public bool Status { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
