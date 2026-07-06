using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_ERD___Models.Models
{
    public class OrderProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Auto-generated
        public int orderProduct { get; set; }   // system generated

        [Required]
        [Range(1, 999)]
        public int quantity { get; set; }  //user input

        [Required]
        [ForeignKey("Order")]
        public int orderId { get; set; }   //Composite Primary Key   //user input

        // Navigation Properties
        public  Order Order { get; set; }  //one order have many order product

        [Required]
        [ForeignKey("product")]
        public int productId { get; set; }  //Composite Primary Key    //user input
        // Navigation Properties
        public Product product { get; set; }  //one Product have many order product

    }
}
