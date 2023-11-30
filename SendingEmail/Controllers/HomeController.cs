using Microsoft.AspNetCore.Mvc;
using SendingEmail.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace SendingEmail.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SendEmail(EmailEntity objEmailParametrs, IFormFile PostedFile)
        {

            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var Username = myAppConfig.GetValue<string>("EmailConfig:Username");
            var Password = myAppConfig.GetValue<string>("EmailConfig:Password");
            var Host = myAppConfig.GetValue<string>("EmailConfig:Host");
            var Port = myAppConfig.GetValue<int>("EmailConfig:Port");
            var FromEmail = myAppConfig.GetValue<string>("EmailConfig:FromEmail");


            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail);
            message.To.Add(objEmailParametrs.ToEmailAddress.ToString());
            message.Subject = objEmailParametrs.EmailSubject;
            message.IsBodyHtml = true;
            message.Body = objEmailParametrs.EmailBodyMessage;


            message.Attachments.Add(new Attachment(PostedFile.OpenReadStream(),
                PostedFile.FileName));
            SmtpClient mailClient = new SmtpClient();

            try
            {                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                mailClient.Host = Host;
                mailClient.Port = Port;
                //mailClient.EnableSsl = false;
                mailClient.Send(message);
                ViewBag.Message = "Successfully Sending Email";

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed Sending Email";
            }
            finally
            {
                mailClient.Dispose();
            }

            return View("Index");
        }

    }
}
