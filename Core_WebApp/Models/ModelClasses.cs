using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_WebApp.Models
{
    public class Category
    {
        [Key] // primary identity Key
        public int CategoryRowId { get; set; }
        [Required(ErrorMessage = "Categry Id Must")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "Categry Name Must")]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Base Price Must")]
        public int BasePrice { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductRowId { get; set; }
        [Required(ErrorMessage = "Product Id Must")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "Product Name Must")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Manufacturer Must")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "Description Must")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price us Must")]
       // [NumericNonNegative(ErrorMessage ="Price cannot be -ve")]
        public int Price { get; set; }
        [ForeignKey("CategoryRowId")]
        public int CategoryRowId { get; set; }
        public Category Category { get; set; }
    }
    public class NumericNonNegativeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (Convert.ToInt32(value) < 0)
            {
                return false;
            }
            return true;
        }
    }
}
