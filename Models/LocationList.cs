using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class LocationList {
		public Models.NewLocation[] lstLocations = new Models.NewLocation[100];
		public ActionTypes ActionType = ActionTypes.NoType;
		public Models.AdminRequest adminReq = new Models.AdminRequest();
		public int  isMember { get; set; }
		public int intRequestNumber { get; set; }

		public LocationList.ActionTypes StoreTempNewLocation(List<Models.CategoryItem>[] categories, List<Models.Days>[] LocationHours, List<Models.SocialMedia>[] socialMedias, List<Models.Website>[] websites, List<Models.ContactPerson>[] contacts) {

			try {
				Database db = new Database();
				Models.User user = new Models.User();
				user = user.GetUserSession();
				
				this.adminReq = new AdminRequest() {
					strRequestType = "INSERT",
					strRequestedChange = "Add Location - " + this.lstLocations[0].StreetAddress + ", " + this.lstLocations[0].City + ' ' + this.lstLocations[0].State + ", " + this.lstLocations[0].Zip,
					intApprovalStatusID = 1,
					intUserID = (short)user.UID
				};

				if (user.isMember == 1) {
					short intMemberID = db.GetMemberID((short)user.UID);
					this.adminReq.intMemberID = intMemberID;
				}

				this.ActionType = db.InsertAdminRequest(this.adminReq);
				if (this.lstLocations[0].CompanyName != string.Empty && this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempCompany(this);
				if (this.ActionType == ActionTypes.InsertSuccessful || this.ActionType == ActionTypes.NoType) this.ActionType = db.InsertTempLocations(this);
				if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempLocationHours(this, LocationHours);
				if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempSpecialties(this, categories);
				if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempSocialMedia(this, socialMedias);
				if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempWebsite(this, websites);
				if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempContactPerson(this, contacts);

				

					

				

				//if something goes bad with new location entry, delete anything related to the new locations entered.
				if (this.ActionType != ActionTypes.InsertSuccessful) {
					int i = 0;
					do {
						if (this.lstLocations[i].lngLocationID != 0 && this.lstLocations[i] != null) {
							db.DeleteLocation(this.lstLocations[i].lngLocationID);
						}
						i++;
					} while (this.lstLocations[i] != null);
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
			return this.ActionType;
		}

		public LocationList.ActionTypes StoreNewLocation(List<Models.CategoryItem>[] categories, List<Models.Days>[] LocationHours, List<Models.SocialMedia>[] socialMedias, List<Models.Website>[] websites, List<Models.ContactPerson>[] contacts, Models.AdminRequest adminRequest) {

				try {
					Database db = new Database();
					if (this.lstLocations[0].CompanyName != string.Empty) this.ActionType = db.InsertCompany(this);
					if (this.ActionType == ActionTypes.InsertSuccessful || this.ActionType == ActionTypes.NoType) this.ActionType = db.InsertLocations(this);
					if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertLocationHours(this, LocationHours);
					if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertSpecialties(this, categories);
					if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertSocialMedia(this, socialMedias);
					if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertWebsite(this, websites);
					if (this.ActionType == ActionTypes.InsertSuccessful) this.ActionType = db.InsertTempContactPerson(this, contacts);

					//if something goes bad with new location entry, delete anything related to the new locations entered.
					if(this.ActionType != ActionTypes.InsertSuccessful) {
					int i = 0;
						do {
							if (this.lstLocations[i].lngLocationID != 0 && this.lstLocations[i] != null) {
								db.DeleteLocation(this.lstLocations[i].lngLocationID);
							}
						i++;
						} while (this.lstLocations[i] != null);
					}

					if(adminRequest.intMemberID != 0) {
					db.InsertCompanyMember(this.lstLocations[0].lngCompanyID, adminRequest.intMemberID);
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			return this.ActionType;
		}

		public enum ActionTypes {
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2,
			DeleteSuccessful = 3,
			RequiredFieldsMissing = 4,
			CompanyFieldsMissing = 5,
			CategoryFieldsMissing = 6,
			HoursFieldsMissing = 7,
			CompanyNameExists = 8,
			LocationHourExists = 9,
			LocationExists = 10,
			CategoryLocationExists = 11,
			SocialMediaExists = 12,
			WebpageURLExists = 13,
			ContactPersonExists = 14,
			DeleteFailed = 15,
			CompanyMemberExist = 16,
			Unknown = 17
		}
	}
}