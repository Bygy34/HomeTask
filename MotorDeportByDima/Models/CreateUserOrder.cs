using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorDeportByDima.Models
{
    public class CreateUserOrder
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(36), MinLength(2)]
        public string StartAddressCity { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string StartAddressRoad { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [MaxLength(36), MinLength(2)]
        public string StartAddressHouse { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string EndAddressCity { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string EndAddressRoad { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string EndAddressHouse { get; set; }

        [Required]
        [MaxLength(36), MinLength(2)]
        public string ProductName { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
    }
}