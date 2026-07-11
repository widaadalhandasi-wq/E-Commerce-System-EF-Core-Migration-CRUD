using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_session_10_SoC.Models
{
   
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderId { get; set; }                  // system generated

        [Required]
        public DateTime orderDate { get; set; }            // system generated — set to today's date

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue)]
        public decimal totalAmount { get; set; }           // calculated — sum of (price * quantity) for all order items

        [Required]
        [MaxLength(30)]
        public string status { get; set; } = "Pending";   // default value — "Pending" | "Processing" | "Shipped" | "Delivered" | "Cancelled"

        [Required]
        [MaxLength(300)]
        public string shippingAddress { get; set; }        // user input

        [Required]
        [MaxLength(50)]
        public string paymentMethod { get; set; }          // from list — "CreditCard" | "DebitCard" | "PayPal" | "Cash"



        /// ////////////////////////////////////////////////////////////////////////////////////////////


        // foreign key — every order must belong to exactly one user
        [Required]
        [ForeignKey("user")]
        public int userId { get; set; }                   // from list — chosen from logged-in user
        public User user { get; set; }                    // navigation property


        // reverse navigation — one Order has many OrderItems (bridge table)
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
