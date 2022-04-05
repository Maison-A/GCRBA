using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;


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
        public ActionResult Index(GCRBA.Models.MailModel _objModelMail) {
            if (ModelState.IsValid) {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                string file = "C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\CSV_Folder\\Bakery.csv";
                var attachment = new Attachment(file, MediaTypeNames.Application.Octet);
                mail.IsBodyHtml = true;
                mail.Attachments.Add(attachment);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("gcrbadata@gmail.com", "sazjptmmfdiyebom");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return RedirectToAction("Index", "Bakery");
            }
            else {
                return View();
			}
        }
    }
}