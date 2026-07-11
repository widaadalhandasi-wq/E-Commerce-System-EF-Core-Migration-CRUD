using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_session_10_SoC.Models
{
    // Bridge entity that resolves the Order <-> Product many-to-many relationship
    // into two one-to-many relationships (Order -> OrderItem, Product -> OrderItem)
    // This entity also holds the relationship attribute: quantity
  
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderItemId { get; set; }              // system generated


        // relationship attribute — carried by this bridge entity
        [Required]
        [Range(1, 999)]
        public int quantity { get; set; }                 // user input










        // foreign key — every order item belongs to exactly one order
        [Required]
        [ForeignKey("Order")]
        public int orderId { get; set; }                  // system generated — from the active order
        public Order Order { get; set; }                  // navigation property

        // foreign key — every order item references exactly one product
        [Required]
        [ForeignKey("Product")]
        public int productId { get; set; }                // from list — chosen from available products
        public Product Product { get; set; }              // navigation property





        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal unitPrice { get; set; }            // calculated — copied from product.price at the time of ordering
    }
}
