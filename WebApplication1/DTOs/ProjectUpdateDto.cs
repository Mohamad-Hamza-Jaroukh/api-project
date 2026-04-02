namespace Portfolio.API.DTOs
{
    public class ProjectUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TechStack { get; set; }

        public string RepoUrl { get; set; }
        public string LiveDemoUrl { get; set; }

        public bool IsFeatured { get; set; }
        public IFormFile? Image { get; set; }
    }
}
