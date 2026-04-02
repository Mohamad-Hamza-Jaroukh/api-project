namespace Portfolio.API.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string subject, string body);
    }
}
