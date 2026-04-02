using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string TechStack { get; set; }

        [MaxLength(200)]
        public string RepoUrl { get; set; }

        [MaxLength(200)]
        public string LiveDemoUrl { get; set; }

        [Required]
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }

    }
}
