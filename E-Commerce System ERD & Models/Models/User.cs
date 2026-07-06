using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace E_Commerce_System_ERD___Models.Models
{
    [Table("User")]
    [Index(nameof(username), IsUnique = true)]  //uniqe
    [Index(nameof(email), IsUnique = true)]  //uniqe
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-generated
        public int  userId {  get; set; } // system generated

        [Required]
        [MaxLength(50)]
        public string username {  get; set; } // user input

        [Required]
        [MaxLength(150)]
        public string email {  get; set; }  // user input

        [Required]
        [MaxLength(256)]
        public string passwordHash {  get; set; }  // user input

        [Required]
        [MaxLength(100)]
        public string fullName {  get; set; } // user input

        [MaxLength(20)]
        public string? phoneNumber {  get; set; }  // user input

        [MaxLength(300)]
        public string? address {  get; set; }  // user input

        [Required]
        public DateTime registrationDate {  get; set; }  // user input

        
        public bool isActive {  get; set; } = true;   //default value

        //one to many 
        public List<Order> userOrders { get; set; }  //Navigation Property => one user can place many orders

        //one to many 
        public List<Review> Reviews { get; set; }  //Navigation Property => one user can write many Reviews
    }
}
