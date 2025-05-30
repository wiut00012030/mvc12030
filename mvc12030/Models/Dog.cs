using System.ComponentModel.DataAnnotations;

namespace mvc12030.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 30)]
        public int Age { get; set; }

        [MaxLength(30)]
        public string Breed { get; set; } = string.Empty;
    }
}
