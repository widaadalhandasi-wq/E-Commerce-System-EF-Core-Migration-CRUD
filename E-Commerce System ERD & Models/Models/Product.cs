using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_ERD___Models.Models
{
    [Table("Product")]
   public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-generated
        public int productId { get; set; }  // system generated

        [Required]
        [MaxLength(150)]
        public string productName { get; set; }  // user input

        [MaxLength(1000)]
        public string? description { get; set; }  // user input

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }  // user input

        [Required]
        [Range(0.0, double.MaxValue)]
        public int stockQuantity { get; set; } = 0;   // user input
      
        [MaxLength(300)]
        public string? imageUrl { get; set; }  // user input

        
        [Required]
        public DateTime createdAt { get; set; }  // user input

        public bool isAvailable { get; set; } = true;   //default value

        //many to many 
        public List<OrderProduct> OrderProducts { get; set; } //Navigation Property => product can hve many order products

        [ForeignKey("Category")]
        public int categoryId { get; set; }  // user input
        //one to many
        public Category Category { get; set; } //Navigation Property => one catogry has many products 
       
        //one to many
        public List<Review> Reviews { get; set; } //Navigation Property => product has many Reviews
    }
}
