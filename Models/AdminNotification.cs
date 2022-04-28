using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class AdminNotification
	{
		public int NotificationID { get; set; }
		public int UserID { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public int EditedColumnID { get; set; }
		public int EditedTableID { get; set; }
		public string EditedColumn { get; set; }
		public string EditedTable { get; set; }
		public string PreviousVersion { get; set; }
		public string NewVersion { get; set; }
		public int NotificationStatusID { get; set; }
		public bool UnreadNotifications { get; set; } = false;
		public string Message { get; set; }
	}
}