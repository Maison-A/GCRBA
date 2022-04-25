using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Models {
	public class MembershipRequestList {
		public List<Models.MembershipRequests> lstMemberRequest = new List<Models.MembershipRequests>();
		public int[] SelectedMemberRequests { get; set; }
		public IEnumerable<SelectListItem> MemberRequests { get; set; }
	}
}