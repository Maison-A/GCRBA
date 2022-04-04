using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;


namespace GCRBA.Controllers
{
    public class SendMailerController : Controller
    {
        // GET: SendMailer
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Index(GCRBA.Models.MailModel _objModelMail) {
            if (ModelState.IsValid) {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                var attachment = new Attachment("C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\CSV_Folder\\Bakery.csv", mediaType: "csv");
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("winslow.shane2@gmail.com", "NuevoEstudiante#3");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else {
                return View();
			}
		}
    }
}