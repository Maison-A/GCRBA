using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class Request
	{
		public User User { get; set; }
		public int RequestID { get; set; }
		public int MemberID { get; set; }
		public int RequestTableID { get; set; }
		public int TempTableID { get; set; }
		public int TotalRequests { get; set; }
	}
}