using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class BakedGoods {
        public Models.CategoryItem Donuts = new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts" };
		public Models.CategoryItem Bagels = new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels" };
		public Models.CategoryItem Muffins = new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins" };
		public Models.CategoryItem IceCream = new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream" };
		public Models.CategoryItem FineCandies = new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates" };
		public Models.CategoryItem WeddingCakes = new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes" };
		public Models.CategoryItem Breads = new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads" };
		public Models.CategoryItem DecoratedCakes = new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes" };
		public Models.CategoryItem Cupcakes = new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes" };
		public Models.CategoryItem Cookies = new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies" };
		public Models.CategoryItem Desserts = new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes" };
		public Models.CategoryItem Full = new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery" };
		public Models.CategoryItem Deli = new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering" };
		public Models.CategoryItem Other = new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli" };
		public Models.CategoryItem Wholesale = new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale" };
		public Models.CategoryItem Delivery = new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)" };
		public Models.CategoryItem Shipping = new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping" };
		public Models.CategoryItem Online = new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering" };

		public List<Models.CategoryItem> lstBakedGoods = new List<CategoryItem>() {
			new Models.CategoryItem() { ItemID = 1, ItemDesc = "Donuts" },
			new Models.CategoryItem() { ItemID = 2, ItemDesc = "Bagels" },
			new Models.CategoryItem() { ItemID = 3, ItemDesc = "Muffins" },
			new Models.CategoryItem() { ItemID = 4, ItemDesc = "Ice Cream" },
			new Models.CategoryItem() { ItemID = 5, ItemDesc = "Fine Candies & Chocolates" },
			new Models.CategoryItem() { ItemID = 6, ItemDesc = "Wedding Cakes" },
			new Models.CategoryItem() { ItemID = 7, ItemDesc = "Breads" },
			new Models.CategoryItem() { ItemID = 8, ItemDesc = "Decorated Cakes" },
			new Models.CategoryItem() { ItemID = 9, ItemDesc = "Cupcakes" },
			new Models.CategoryItem() { ItemID = 10, ItemDesc = "Cookies" },
			new Models.CategoryItem() { ItemID = 11, ItemDesc = "Desserts/Tortes" },
			new Models.CategoryItem() { ItemID = 12, ItemDesc = "Full-line Bakery" },
			new Models.CategoryItem() { ItemID = 13, ItemDesc = "Deli/Catering" },
			new Models.CategoryItem() { ItemID = 14, ItemDesc = "Other Carryout Deli" },
			new Models.CategoryItem() { ItemID = 15, ItemDesc = "Wholesale" },
			new Models.CategoryItem() { ItemID = 16, ItemDesc = "Delivery (3rd Party)" },
			new Models.CategoryItem() { ItemID = 17, ItemDesc = "Shipping" },
			new Models.CategoryItem() { ItemID = 18, ItemDesc = "Online Ordering" }
	};


	}
}