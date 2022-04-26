using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class Notification
	{
		public int NotificationID { get; set; }
		public string Message { get; set; }
		public int NotificationStatusID { get; set; }
		public string NotificationStatus { get; set; }
		public bool UnreadNotifications { get; set; } = false;
	}
}