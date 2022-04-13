using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class SearchResults
	{
		
		public int test = 0;
	
		public List<Models.NewLocation> lstLocations;
		public int testLocationSearch = 1;
		public Models.NewLocation landingLocation;
		public List<Models.CategoryItem> landingCategories;
		public List<Models.Awards> landingAwards;
		public List<Models.SaleSpecial> landingSpecials;
		public List<Models.ContactPerson> landingContacts;
		public List<Models.SocialMedia> landingSocialMedia;
		public List<Models.Website> landingWebsite;

		//Bakery Good For User Search
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
	}
}