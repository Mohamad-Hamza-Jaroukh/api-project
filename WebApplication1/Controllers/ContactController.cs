using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data;
using Portfolio.API.DTOs;
using Portfolio.API.Models;
using Portfolio.API.Services;

namespace Portfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public ContactController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = new ContactMessage
            {
                Name = dto.Name,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message
            };

            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();

            try
            {
                var emailBody = $@"
New Contact Message:

Name: {dto.Name}
Email: {dto.Email}
Subject: {dto.Subject}

Message:
{dto.Message}
";

                await _emailService.SendEmailAsync(
                    $"Portfolio Contact: {dto.Subject}",
                    emailBody
                );
            }
            catch
            {
                return StatusCode(500, "Message saved but email failed to send.");
            }

            return Ok("Message sent successfully.");
        }
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            return Ok(messages);
        }   
    }
}