using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models
{
	public class SaleSpecial
	{
		public string strDescription = string.Empty;
		public decimal monPrice { get; set; }
		public DateTime dtmStart { get; set; }
		public DateTime dtmEnd { get; set; }
	}
}