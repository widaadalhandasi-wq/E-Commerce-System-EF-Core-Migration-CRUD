using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_System_ERD___Models.Models
{
    [Table("Category")]
    [Index(nameof(categoryName), IsUnique = true)]  //uniqe
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Auto-generated
        public int categoryId {  get; set; }  // system generated

        [Required]
        [MaxLength(100)]
        public string categoryName { get; set; }  // user input

        [MaxLength(500)]
        public string? description { get; set; }  // user input

        [MaxLength(300)]
        public string? imageUrl { get; set; }  // user input


        //one to many 
        public List<Product> products { get; set; }   //Navigation properties   => one catogray has many products


    }
}
