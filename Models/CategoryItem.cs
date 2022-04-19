using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GCRBA.Models {
	public class CategoryItem {
		public long lngCategoryLocationID = 0;
		public int ItemID = 0;
		public string ItemDesc = string.Empty;
		public bool blnAvailable = false;
        public CategoryItem.ActionTypes ActionType = ActionTypes.NoType;

        public enum ActionTypes
        {
            NoType = 0,
            InsertSuccessful = 1,
            UpdateSuccessful = 2,
            DeleteSuccessful = 3,
            Unknown = 4,
            RequiredFieldMissing = 5
        }
    }
}