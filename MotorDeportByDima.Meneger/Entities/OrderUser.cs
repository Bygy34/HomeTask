using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Entities
{
    public class OrderUser
    {
        public string Id { get; set; }

        public bool Status { get; set; }

        public string StartAddressCity { get; set; }

        public string StartAddressRoad { get; set; }

        public string StartAddressHouse { get; set; }

        public string EndAddressCity { get; set; }

        public string EndAddressRoad { get; set; }

        public string EndAddressHouse { get; set; }

 //       public string StartAddress { get; set; }

        public DateTime StartDate { get; set; }

 //       public string EndAddress { get; set; }

        public DateTime DueDate { get; set; }

        public string ProductName { get; set; }

        public int Weight { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }


        
    }
}
