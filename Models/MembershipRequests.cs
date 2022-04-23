using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class MembershipRequests {
		public short intMemberID { get; set; }
		public short intUserID { get; set; }
		public short intMembershipLevelID { get; set; }
		public short intPaymentTypeID { get; set; }
		public short intPaymentStatusID { get; set; }
		public short intApprovalStatusID { get; set; }
		public string strFirstName { get; set; }
		public string strLastName { get; set; }
		public string strPhone { get; set; }
		public string strEmail { get; set; }
	}
}