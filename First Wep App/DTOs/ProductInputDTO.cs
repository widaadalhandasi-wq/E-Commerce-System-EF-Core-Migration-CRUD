using System.ComponentModel.DataAnnotations;

namespace FirstWebApp.DTOs
{
    public class ProductInputDTO
    {
        [Required(ErrorMessage = "Value should not be null.")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int Price { get; set; }
    }
}
