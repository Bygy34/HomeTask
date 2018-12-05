using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorDeportByDima.Models
{
    public class CreateDriverOrder
    {

        public string Id { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string StartAddressCity { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string StartAddressRoad { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string StartAddressHouse { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string CarBrand { get; set; }

        [Required]
        [Range(0, 100000)]
        public int MaxWeight { get; set; }
    }
}