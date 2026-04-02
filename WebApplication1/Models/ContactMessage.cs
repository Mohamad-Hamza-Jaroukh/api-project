using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Subject { get; set; }

        [Required, MaxLength(1000)]
        public string Message { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}