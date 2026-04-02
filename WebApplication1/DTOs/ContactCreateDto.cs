using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DTOs
{
    public class ContactCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Subject { get; set; }

        [Required, MaxLength(1000)]
        public string Message { get; set; }
    }
}