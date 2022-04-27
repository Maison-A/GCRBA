using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;

namespace GCRBA.Models {
	public class SendMemberEmail {
		static void Main(Models.User user) {
			SendEmail(user);
		}

		public static void SendEmail(Models.User user) {
			GCRBA.Models.MemberMailModel memberMailModel = new Models.MemberMailModel();
			memberMailModel.Content = new Models.User();
			memberMailModel.Content.Email = user.Email;
			memberMailModel.Content.userPhone = new Models.PhoneNumber();
			memberMailModel.Content.userPhone.AreaCode = user.userPhone.AreaCode;
			memberMailModel.Content.userPhone.Prefix = user.userPhone.Prefix;
			memberMailModel.Content.userPhone.Suffix = user.userPhone.Suffix;
			memberMailModel.Content.Address = user.Address;
			memberMailModel.Content.City = user.City;
			memberMailModel.Content.State = user.State;
			memberMailModel.Content.Zip = user.Zip;
			memberMailModel.Content.FirstName = user.FirstName;
			memberMailModel.Content.LastName = user.LastName;
			memberMailModel.Content.MemberShipType = user.MemberShipType;
			memberMailModel.Content.PaymentType = user.PaymentType;

			if (!String.IsNullOrEmpty(memberMailModel.Content.userPhone.AreaCode) && !String.IsNullOrEmpty(memberMailModel.Content.userPhone.Prefix) && !String.IsNullOrEmpty(memberMailModel.Content.userPhone.Suffix)) {
				memberMailModel.Content.Phone = '(' + memberMailModel.Content.userPhone.AreaCode + ") " + memberMailModel.Content.userPhone.Prefix + '-' + memberMailModel.Content.userPhone.Suffix;
			}

			if (!String.IsNullOrEmpty(memberMailModel.Content.FirstName) && !String.IsNullOrEmpty(memberMailModel.Content.LastName)) {
				memberMailModel.UserFullName = memberMailModel.Content.LastName + ", " + memberMailModel.Content.FirstName;
			}

			string body = string.Empty;
			using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~\\Views\\Shared\\MemberEmailTemplate.html"))) {
				//"C:\\Users\\winsl\\OneDrive\\Desktop\\Capstone\\MVC\\Views\\Shared\\LocEmailTemplate.html"
				//"E:\\Web-Folders\\Students\\Spring\\CPDM-290-200\\CPDM-WinslowS\\Views\\Shared\\LocEmailTemplate.html"
				//Server.MapPath("~\\Views\\Shared\\LocEmailTemplate.html")
				body = reader.ReadToEnd();
			}

			body = body.Replace("{UserFullName}", memberMailModel.UserFullName);
			body = body.Replace("{UserEmail}", memberMailModel.Content.Email);
			body = body.Replace("{Address}", memberMailModel.Content.Address);
			body = body.Replace("{City}", memberMailModel.Content.City);
			body = body.Replace("{State}", memberMailModel.Content.State);
			body = body.Replace("{Zip}", memberMailModel.Content.Zip);
			body = body.Replace("{MemberShipType}", memberMailModel.Content.MemberShipType);
			body = body.Replace("{PaymentType}", memberMailModel.Content.PaymentType);
			body = body.Replace("{Phone}", memberMailModel.Content.Phone);
			body = body.Replace("{Description}", memberMailModel.Description);

			using (MailMessage mailMessage = new MailMessage()) {
				mailMessage.From = new MailAddress(memberMailModel.UserName);
				mailMessage.Subject = memberMailModel.Subject;
				mailMessage.Body = body;
				mailMessage.IsBodyHtml = true;
				mailMessage.To.Add(new MailAddress(memberMailModel.Recipient));
				SmtpClient smtp = new SmtpClient();
				smtp.Host = "smtp.gmail.com";
				smtp.Port = 587;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new System.Net.NetworkCredential("gcrbadata@gmail.com", "sazjptmmfdiyebom");
				smtp.EnableSsl = true;
				smtp.Send(mailMessage);
			}
		}
	}
}