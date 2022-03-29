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
		public string Contact_FirstName = string.Empty;
		public string Contact_LastName = string.Empty;
		public PhoneNumber ContactPhone;
		public PhoneNumber BusinessPhone;
		public string Email = string.Empty;
		public string Website = string.Empty;

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
		
		/*
		public string Bagels = string.Empty;
		public string Muffins = string.Empty;
		public string IceCream = string.Empty;
		public string FineCandies = string.Empty;
		public string WeddingCakes = string.Empty;
		public string Breads = string.Empty;
		public string DecoratedCakes = string.Empty;
		public string Cupcakes = string.Empty;
		public string Cookies = string.Empty;
		public string Desserts = string.Empty;
		public string Full = string.Empty;
		public string Deli = string.Empty;
		public string CarryoutDeli = string.Empty;
		public string Wholesale = string.Empty;
		public string Delivery = string.Empty;
		public string Shipping = string.Empty;
		public string Online = string.Empty;
		*/

		public ActionTypes ActionType = ActionTypes.NoType;

		//!!!!!!!!!!!!if they are a member they will have these variables available to them!!!!!!!!!!!!!!!!!!
		public string custServiceEmail = string.Empty;
		//Some variables related to Current Promotional Information
		public string Bio = string.Empty;
		public string KettleLink = string.Empty;
		public string OnlineOrdering = string.Empty;
		
		//Social Media Variables that will be allowed to fill out if member identifies
		public string Facebook = string.Empty;
		public string Twitter = string.Empty;
		public string Istagram = string.Empty;
		public string Snapchat = string.Empty;
		public string TikTok = string.Empty;
		public string Yelp = string.Empty;

		public enum ActionTypes {
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2,
			DeleteSuccessful = 3,
			RequiredFieldsMissing = 4,
			Unknown = 5
		}

		public NewLocation.ActionTypes StoreNewLocation(List<Models.CategoryItem> categories, List<Models.Days> LocationHours) {
			try {
				Database db = new Database();
				this.ActionType = db.InsertCompany(this);
				this.ActionType = db.InsertLocation(this);
				this.ActionType = db.InsertSpecialties(this, categories);
				this.ActionType = db.InsertLocationHours(this, LocationHours);
				return this.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}
	}
}