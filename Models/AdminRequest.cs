using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class AdminRequest {
		public short intAdminRequest { get; set; }
		public short intMemberID { get; set; }
		public short intUserID { get; set; }
		public string strRequestType { get; set; }
		public string strRequestedChange { get; set; }
		public short intApprovalStatusID { get; set; }
		public int intSelection { get; set; }
	}
}