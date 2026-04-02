using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Models
{
    public class Profile
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string LinkedIn { get; set; }

        [MaxLength(200)]
        public string GitHub { get; set; }
    }
}
