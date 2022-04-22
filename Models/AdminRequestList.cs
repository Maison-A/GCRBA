using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GCRBA.Models {
	public class AdminRequestList {
		public List<Models.AdminRequest> lstAdminRequest = new List<Models.AdminRequest>();
		public int[] SelectedAdminRequests { get; set; }
		public IEnumerable<SelectListItem> AdminRequests { get; set; }
	}
}