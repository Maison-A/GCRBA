using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class LocationList {
		public Models.NewLocation[] lstLocations = new Models.NewLocation[100];
		public ActionTypes ActionType = ActionTypes.NoType;
		public int  isMember { get; set; }

		public LocationList.ActionTypes StoreNewLocation(List<Models.CategoryItem>[] categories, List<Models.Days>[] LocationHours, List<Models.SocialMedia>[] socialMedias, List<Models.Website>[] websites, List<Models.ContactPerson>[] contacts) {

				try {
					Database db = new Database();
					if (this.lstLocations[0].CompanyName != string.Empty) this.ActionType = db.InsertCompany(this);
					if (this.ActionType != ActionTypes.CompanyNameExists) this.ActionType = db.InsertLocations(this);
					if (this.ActionType != ActionTypes.LocationExists) this.ActionType = db.InsertLocationHours(this, LocationHours);
					if (this.ActionType != ActionTypes.LocationHourExists) this.ActionType = db.InsertSpecialties(this, categories);
					if (this.ActionType != ActionTypes.CategoryLocationExists) this.ActionType = db.InsertSocialMedia(this, socialMedias);
					if (this.ActionType != ActionTypes.SocialMediaExists) this.ActionType = db.InsertWebsite(this, websites);
					if (this.ActionType != ActionTypes.WebpageURLExists) this.ActionType = db.InsertContactPerson(this, contacts);

					//if something goes bad with new location entry, delete anything related to the new locations entered.
					if(this.ActionType != ActionTypes.InsertSuccessful) { db.DeleteLocations(this); }
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
			Unknown = 16
		}
	}
}