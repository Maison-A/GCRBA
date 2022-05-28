using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace GCRBA.Models {
	public class ContactPerson {
		// contact person ID
		public long ContactPersonID { get; set; } = 0;

		// contact name
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;

		// contact phone
		public PhoneNumber ContactPhone { get; set; }
		public string Phone { get; set; } = string.Empty;

		// contact email
		public string Email { get; set; } = string.Empty;

		// contact location 
		public Location Location { get; set; }

		// contact type
		public short ContactTypeID { get; set; } = 0;
		public string ContactPersonType { get; set; }

		// contact company 
		public Company Company { get; set; }
		public List<string> Types { get; set; }
		 
		// enums 
		public ContactPerson.ContactTypes ContactType = ContactTypes.NoType;
		public ContactPerson.ActionTypes ActionType = ActionTypes.NoType;

		// create new contact object for new contact being added to database by admin 
		public ContactPerson CreateContact(string FirstName = "", string LastName = "", string Phone = "", string Email = "", long LocationID = 0, short TypeID = 0)
		{
			this.Location = new Location();
			this.Company = new Company();
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Phone = Phone;
			this.Email = Email;
			this.Location.LocationID = LocationID;
			this.ContactTypeID = TypeID;
			this.Company = Company.GetCompanySession();

			return this;
		}

		public ActionTypes UpdateContact(string FirstName = "", string LastName = "", string Phone = "", string Email = "", long ContactID = 0)
		{

			// get contact so we can compare 
			this.ContactPersonID = ContactID;
			this.Location = new Location();
			this.GetSpecificContact();

			if (FirstName != this.FirstName || LastName != this.LastName || Phone != this.Phone || Email != this.Email)
			{
				// phone can be null in db so if field is empty, 
				// update phone to null in db 
				if (string.IsNullOrEmpty(Phone))
				{
					this.Phone = null;
				}
				else 
				{
					this.Phone = Phone;
				}

				// email can be null in db so if field is empty, 
				// update email to null in db 
				if (string.IsNullOrEmpty(Email))
				{
					this.Email = null;
				}
				else
				{
					this.Email = Email;
				}

				// if first name field is empty, we keep current first name 
				if (!string.IsNullOrEmpty(FirstName))
				{
					this.FirstName = FirstName;
				}

				// if last name field is empty, we keep current last name 
				if (!string.IsNullOrEmpty(LastName))
				{
					this.LastName = LastName;
				}

				// combine first and last name in proper format 
				this.FullName = FormatName();

				// get current company session 
				this.Company = new Company();
				this.Company = Company.GetCompanySession();

				// submit to database 
				return  SubmitContactUpdate();
			}
			else
			{
				return ActionTypes.NoType;
			}
		}

		public ActionTypes SubmitContactUpdate()
		{
			Database db = new Database();
			return db.UpdateContact(this);
		}

		public ActionTypes ValidateNewContactForm()
		{
			// create list to hold potential list of action types 
			List<ContactPerson.ActionTypes> ActionTypeList = new List<ContactPerson.ActionTypes>();

			// are any of the required fields empty/not selected?
			if (!string.IsNullOrEmpty(this.FirstName) && !string.IsNullOrEmpty(this.LastName) && this.Location.LocationID > 0 && this.ContactTypeID > 0)
			{

				// format name for database submission 
				this.FullName = FormatName();

				// required fields aren't empty, submit new contact to db  
				return AddNewContact();

			} else
			{
				return ActionTypes.RequiredFieldsMissing;
			}
		}

		public string FormatName()
		{
			this.FullName = this.LastName + ", " + this.FirstName;
			return this.FullName;
		}

		public string GetFirstName()
		{
			// get index of comma 
			int index = 0;
			index = this.FullName.IndexOf(',');

			// we don't want the comma nor space included, so add 2 to the index
			index += 2;

			this.FirstName = this.FullName.Substring(index);

			return this.FirstName;
		}

		public string GetLastName()
		{
			// get index of comma
			int index = 0;

			index = this.FullName.IndexOf(',');

			this.LastName = this.FullName.Substring(0, index);

			return this.LastName;
		}

		public ActionTypes AddNewContact()
		{
			Database db = new Database();
			return db.InsertNewContact(this);
		}

		public List<ContactPerson> GetContactsByLocation(int LocationID)
		{
			Database db = new Database();
			return db.GetContactsByLocation(LocationID);
		}

		public List<ContactPerson> GetContactsByCompany(Company c)
		{
			Database db = new Database();
			return db.GetContactsByCompany(c);
		}

		public ContactPerson GetSpecificContact()
		{
			Database db = new Database();
			return db.GetSpecificContact(this);
		}

		public List<string> GetContactTypes()
		{
			Database db = new Database();
			return db.GetContactTypes();
		}

		public enum ActionTypes
        {
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2, 
			DeleteSuccessful = 3,
			Unknown = 4, 
			RequiredFieldsMissing = 5,
			PhoneFormatIssue = 6,
			DuplicateName = 7
		}

		public enum ContactTypes {
			LocationContact = 1,
			CustomerService = 2,
			WebAdmin = 3,
			NoType = 4
		}
	}
}