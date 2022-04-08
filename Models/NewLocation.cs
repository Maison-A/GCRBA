using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class NewLocation {
		public long lngLocationID = 0;
		public long lngCompanyID = 0;
		public List<Models.Company> lstCompanies = new List<Models.Company>();

		//Address Information
		public string CompanyName = string.Empty;
		public string LocationName = "Grottees Bakery";//string.Empty;
		public string StreetAddress = "292 Rampart Ct."; //string.Empty;
		public string City = "Ft. Mitchell"; // string.Empty;
		public string State = "OH"; //string.Empty;
		public string Zip = "41017"; //string.Empty;

		//NEED TO CHANGE TO INT FOR STATE
		public short intState = 0;
		public List<Models.State> lstStates = new List<Models.State>();

		//Contact Information
		public PhoneNumber BusinessPhone;
		public string strFullPhone = string.Empty;
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

		public string selectedGood = string.Empty;

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
		public string Bio = "This is a test of your regular broadcasting service. This is only a test. So don't freak out man!"; //HttpContext.Current.Server.MapPath("/Bakery.csv");
		public string BizYear = "1988";  //string.Empty;
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
				if(this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertLocation(this);
				if(this.ActionType != ActionTypes.Unknown) //this.ActionType = db.InsertLocationHours(this, LocationHours);
				if(this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertSpecialties(this, categories);
				if(this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertSocialMedia(this, socialMedias);
				if(this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertWebsite(this, websites);
				if(this.ActionType != ActionTypes.Unknown) this.ActionType = db.InsertContactPerson(this, contacts);
				return this.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}
	}
}