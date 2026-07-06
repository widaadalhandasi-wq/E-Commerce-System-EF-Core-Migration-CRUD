using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_ERD___Models.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Auto-generated
        public int reviewId { get; set; }  // system generated

        [Required]
        [Range(1,5)]
        public int rating { get; set; }   // user input


        [MaxLength(1000)]
        public string? comment { get; set; }    // user input


        [Required]
        public DateTime reviewDate { get; set; }   // user input

        [Required]
        [ForeignKey("Product")]
        public int productId { get; set; }   // user input
        //one to many
        public Product Product { get; set; } //Navigation Property => reviewes for one product

        [Required]
        [ForeignKey("User")]
        public int userId { get; set; }   // user input
        //one to many
        public User User { get; set; } //Navigation Property => reviewes for one user
    }
}
