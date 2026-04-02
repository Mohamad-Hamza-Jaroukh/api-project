using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.Models;

namespace Portfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VisitsController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/visits
        [HttpPost]
        public async Task<IActionResult> AddVisit()
        {
            var visit = new Visit();
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/visits/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetVisitCount()
        {
            var count = await _context.Visits.CountAsync();
            return Ok(count);
        }
    }
}
