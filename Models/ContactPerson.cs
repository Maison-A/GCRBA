using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class ContactPerson {
		public long lngContactPersonID = 0;
		public string strContactFirstName = string.Empty;
		public string strContactLastName = string.Empty;
		public PhoneNumber contactPhone;
		public string strContactEmail = string.Empty;
		public int intContactTypeID = 0;

		public string strFullName = string.Empty;
		public string strFullPhone = string.Empty;
	}
}