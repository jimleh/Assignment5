using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.Models
{
    public class StockItem
    {
        [Key]
        [Display(Name = "#")]
        public int ArticleNumber { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 100000)]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double Price { get; set; }
        [Display(Name = "Shelf")]
        public string ShelfPosition { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}