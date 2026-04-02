using Portfolio.API.Services;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(
                _config["EmailSettings:SenderEmail"],
                _config["EmailSettings:AppPassword"]
            ),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:SenderEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        mailMessage.To.Add(_config["EmailSettings:ReceiverEmail"]);

        await smtpClient.SendMailAsync(mailMessage);
    }
}