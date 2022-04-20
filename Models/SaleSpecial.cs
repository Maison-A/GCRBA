using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class SaleSpecial
	{
		public int SpecialID { get; set; }
		public string strDescription = string.Empty;
		public decimal monPrice { get; set; }
		public DateTime dtmStart { get; set; }
		public DateTime dtmEnd { get; set; }
		public SaleSpecial.ActionTypes ActionType { get; set; } = ActionTypes.NoType;

		public enum ActionTypes
		{
			NoType = 0,
			InsertSuccessful = 1, 
			UpdateSuccessful = 2, 
			DeleteSuccessful = 3, 
			Unknown = 4
		}
	}
}