﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class ContactPerson {
		public long intContactPersonID = 0;
		public string strContactFirstName = string.Empty;
		public string strContactLastName = string.Empty;
		public PhoneNumber contactPhone;
		public string strContactEmail = string.Empty;
	}
}