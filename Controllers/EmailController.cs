using EmailMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace EmailMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmailController(IConfiguration  config)
        {
            _config = config;
        }

        [HttpPost]

        public async Task<IActionResult> SendEmail(Message message)
        {
            String SendMailFrom = _config.GetSection("EmailUsername").Value;
            String SendMailTo = message.To;
            String SendMailSubject = message.Subject;
            String SendMailBody = message.Body;

            try
            {
                SmtpClient SmtpServer = new SmtpClient(_config.GetSection("EmailHost").Value, 587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();
                // START
                email.From = new MailAddress(SendMailFrom);
                email.To.Add(SendMailTo);
                email.CC.Add(SendMailFrom);
                email.Subject = SendMailSubject;
                email.Body = SendMailBody;
                email.IsBodyHtml = true;
                if (message.Priority == true) { email.Priority = MailPriority.High; }

                //END
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, _config.GetSection("EmailPassword").Value);
                SmtpServer.Send(email);

                return Ok("Email Successfully Sent");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
