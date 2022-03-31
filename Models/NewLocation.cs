using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class NewLocation {
		public long lngLocationID = 0;
		public long lngCompanyID = 0;

		//Address Information
		public string LocationName = string.Empty;
		public string StreetAddress = string.Empty;
		public string City = string.Empty;
		public string State = string.Empty;
		public string Zip = string.Empty;

		//NEED TO CHANGE TO INT FOR STATE
		public int intState = 0;

		//Contact Information
		public PhoneNumber BusinessPhone;
		public string BusinessEmail = string.Empty;

		//Bakery Specialty Information
		public CategoryItem Donuts = new CategoryItem();
		public CategoryItem Bagels = new CategoryItem();
		public CategoryItem Muffins = new CategoryItem();
		public CategoryItem IceCream = new CategoryItem();
		public CategoryItem FineCandies = new CategoryItem();
		public CategoryItem WeddingCakes = new CategoryItem();
		public CategoryItem Breads = new CategoryItem();
		public CategoryItem DecoratedCakes = new CategoryItem();
		public CategoryItem Cupcakes = new CategoryItem();
		public CategoryItem Cookies = new CategoryItem();
		public CategoryItem Desserts = new CategoryItem();
		public CategoryItem Full = new CategoryItem();
		public CategoryItem Deli = new CategoryItem();
		public CategoryItem Other = new CategoryItem();
		public CategoryItem Wholesale = new CategoryItem();
		public CategoryItem Delivery = new CategoryItem();
		public CategoryItem Shipping = new CategoryItem();
		public CategoryItem Online = new CategoryItem();

		//Bakery Days/Times Hours of Operation
		public Days Sunday = new Days();
		public Days Monday = new Days();
		public Days Tuesday = new Days();
		public Days Wednesday = new Days();
		public Days Thursday = new Days();
		public Days Friday = new Days();
		public Days Saturday = new Days();

		public ActionTypes ActionType = ActionTypes.NoType;

		//!!!!!!!!!!!!if they are a member they will have these variables available to them!!!!!!!!!!!!!!!!!!
		public ContactPerson LocationContact;
		public ContactPerson WebAdmin;
		public ContactPerson CustService;
		public string custServiceEmail = string.Empty;

		//Some variables related to Current Promotional Information
		public string Bio = string.Empty;
		public string BizYear = string.Empty;
		public Website MainWeb;
		public Website OrderingWeb;
		public Website KettleWeb;

		//Social Media Variables that will be allowed to fill out if member identifies
		public SocialMedia Facebook;
		public SocialMedia Twitter;
		public SocialMedia Instagram;
		public SocialMedia Snapchat;
		public SocialMedia TikTok;
		public SocialMedia Yelp;

		public enum ActionTypes {
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2,
			DeleteSuccessful = 3,
			RequiredFieldsMissing = 4,
			Unknown = 5
		}

		public NewLocation.ActionTypes StoreNewLocation(List<Models.CategoryItem> categories, List<Models.Days> LocationHours, List<Models.SocialMedia> socialMedias, List<Models.Website> websites, List<Models.ContactPerson> contacts) {
			try {
				Database db = new Database();
				this.ActionType = db.InsertCompany(this);
				this.ActionType = db.InsertLocation(this);
				this.ActionType = db.InsertSpecialties(this, categories);
				this.ActionType = db.InsertLocationHours(this, LocationHours);
				this.ActionType = db.InsertSocialMedia(this, socialMedias);
				this.ActionType = db.InsertWebsite(this, websites);
				this.ActionType = db.InsertContactPerson(this, contacts);
				return this.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}
	}
}