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
		public string strContactPersonType = string.Empty;

		public ContactPerson.ActionTypes ActionType = ActionTypes.NoType;

		public enum ActionTypes
        {
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2, 
			DeleteSuccessful = 3,
			Unknown = 4, 
			RequiredFieldsMissing = 5
        }
	}
}