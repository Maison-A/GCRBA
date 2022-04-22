using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class NewLocation {
		public long lngLocationID = 0;
		public long lngCompanyID = 0;
		public List<Models.Company> lstCompanies { get; set; }
		public bool memberStatus { get; set; }

		//Address Information
		public string CompanyName = string.Empty;
		public string LocationName = string.Empty;
		public string StreetAddress = string.Empty;
		public string City = string.Empty;
		public string State = string.Empty;
		public string Zip = string.Empty;

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

		public Models.BakedGoods bakedGoods = new Models.BakedGoods();

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
		public string Bio = string.Empty; //HttpContext.Current.Server.MapPath("/Bakery.csv");
		public string BizYear = string.Empty;  //string.Empty;
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
			CompanyFieldsMissing = 5,
			CategoryFieldsMissing = 6,
			Unknown = 7
		}


	}
}