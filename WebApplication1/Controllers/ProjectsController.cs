using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }
 
        [HttpPost("with-image")]
        public async Task<IActionResult> CreateProjectWithImage([FromForm] ProjectCreateWithImageDto dto)
        {
            string imageName = null;

            if (dto.Image != null)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/projects"
                );

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                imageName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, imageName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);
            }

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                TechStack = dto.TechStack,
                RepoUrl = dto.RepoUrl,
                LiveDemoUrl = dto.LiveDemoUrl,
                IsFeatured = dto.IsFeatured,
                ImageUrl = imageName
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project Id {id} is not exstes...!");
            return Ok(project);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"Project Id {id} is not exists...!");

            // 🖼️ حذف الصورة من السيرفر
            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                var imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/projects",
                    project.ImageUrl
                );

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects = await _context.Projects
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    TechStack = p.TechStack,
                    RepoUrl = p.RepoUrl,
                    LiveDemoUrl = p.LiveDemoUrl,
                    IsFeatured = p.IsFeatured,
                    ImageUrl=p.ImageUrl
                })
                .ToListAsync();

            return Ok(projects);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id,[FromForm] ProjectUpdateDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound($"project id {id} is not exests...!");

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.TechStack = dto.TechStack;
            project.RepoUrl = dto.RepoUrl;
            project.LiveDemoUrl = dto.LiveDemoUrl;
            project.IsFeatured = dto.IsFeatured;

            // 🖼️ إذا في صورة جديدة
            if (dto.Image != null)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/projects"
                );

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // حذف الصورة القديمة
                if (!string.IsNullOrEmpty(project.ImageUrl))
                {
                    var oldImagePath = Path.Combine(uploadsFolder, project.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                // حفظ الصورة الجديدة
                var newImageName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                var newImagePath = Path.Combine(uploadsFolder, newImageName);

                using var stream = new FileStream(newImagePath, FileMode.Create);
                await dto.Image.CopyToAsync(stream);

                project.ImageUrl = newImageName;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
