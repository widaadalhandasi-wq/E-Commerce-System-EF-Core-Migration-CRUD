using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_session_10_SoC.Models
{
 
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int reviewId { get; set; }                 // system generated

        [Required]
        [Range(1, 5)]
        public int rating { get; set; }                   // user input — 1 to 5 stars

        [MaxLength(1000)]
        public string comment { get; set; }               // user input

        [Required]
        public DateTime reviewDate { get; set; }           // system generated — set to today's date




        // foreign key — every review is written by exactly one user
        [Required]
        [ForeignKey("U")]
        public int userId { get; set; }                   // from list — from logged-in user
        public virtual User U { get; set; }                    // navigation property



        // foreign key — every review is about exactly one product
        [Required]
        [ForeignKey("product")]
        public int productId { get; set; }                // from list — chosen from purchased products
        public virtual Product product { get; set; }              // navigation property
    }
}
