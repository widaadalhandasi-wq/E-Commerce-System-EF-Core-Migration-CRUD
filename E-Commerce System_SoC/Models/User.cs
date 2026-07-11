using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_session_10_SoC.Models
{

   [Index(nameof(Name), nameof(email), IsUnique = true)]
    public class User
    {
        [Key] //by default unique and not null no need to add [Required] attribute
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }                  // system generated

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }              // user input

        [Required]
        [MaxLength(150)]
        public string email { get; set; }                 // user input

        [Required]
        [MaxLength(256)]
        public string passwordHash { get; set; }          // system generated — hashed from user input

        [Required]
        [MaxLength(100)]
        public string fullName { get; set; }              // user input

        [MaxLength(20)]
        public string phoneNumber { get; set; }           // user input

        [MaxLength(300)]
        public string? address { get; set; }               // user input

        //? registrationDate is nullable because it will be set to today's date when the user is created, but it may not be set yet if the user is not created yet.
        public DateTime? registrationDate { get; set; }    // system generated — set to today's date

        public bool isActive { get; set; } = true;        // default value

        /// /////////////////////////////////////////////////////////////////////////////

        // reverse navigation — one User writes many Reviews
        public List<Review> Reviews { get; set; }



        // reverse navigation — one User places many Orders
        public List<Order> Orders { get; set; }


    }
}
