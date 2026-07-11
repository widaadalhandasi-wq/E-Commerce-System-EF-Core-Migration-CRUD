using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_session_10_SoC.Models
{
 
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int productId { get; set; }                // system generated

        [Required]
        [MaxLength(150)]
        public string productName { get; set; }            // user input

        [MaxLength(1000)]
        public string description { get; set; }            // user input

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue)]
        public double price { get; set; }                 // user input

        [Required]
        [Range(0, int.MaxValue)]
        public int stockQuantity { get; set; } = 0;        // default value — updated on orders

        [MaxLength(300)]
        public string imageUrl { get; set; }               // user input

        [Required]
        public DateTime createdAt { get; set; }            // system generated — set to today's date

        public bool isAvailable { get; set; } = true;      // default value

        /////////////////////////////////////////////////////////////////////////////////




        // foreign key — every product must belong to a category
        [Required]
        [ForeignKey("c")]
        public int categoryId { get; set; }                // from list — chosen from categories list
        public virtual Category c { get; set; }             // navigation property


        // reverse navigation — one Product has many Reviews
        public virtual List<Review> Reviews { get; set; } = new List<Review>();



        // reverse navigation — one Product appears in many OrderItems (bridge table)
        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
