using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDeportByDima.Meneger.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string Status { get; set; }

        public virtual OrderUser OrderUser { get; set; }
        public string OrderUserId { get; set; }

        public virtual OrderDriver OrderDriver { get; set; }
        public string OrderDriverId { get; set; }
    }
}
