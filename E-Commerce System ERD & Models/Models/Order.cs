using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_ERD___Models.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Auto-generated
        public int orderId { get; set; }  // system generated


        [Required]
        public DateTime orderDate { get; set; }   // user input


        [Required]
        [Range(0.0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal totalAmount { get; set; }   // user input


        [Required]
        [MaxLength(30)]
        public orderSatus status { get; set; } = orderSatus.Pending; // default value 

        [Required]
        [MaxLength(300)]
        public string shippingAddress { get; set; }   // user input


        [Required]
        [MaxLength(50)]
        public string paymentMethod { get; set; }    // user input


        [Required]
        [ForeignKey("user")]
        public int userId { get; set; }   // user input
        //one to many 
        public User user { get; set; } //Navigation Property =>order must be associated with exactly one user

        //many to many 
        public  List<OrderProduct> OrderProducts { get; set; } //Navigation Property => order can hve many order products
    }
}
