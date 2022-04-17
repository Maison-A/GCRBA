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
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertLocations(this);
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertLocationHours(this, LocationHours);
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertSpecialties(this, categories);
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertSocialMedia(this, socialMedias);
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertWebsite(this, websites);
					if (this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertContactPerson(this, contacts);
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
			Unknown = 8
		}
	}
}