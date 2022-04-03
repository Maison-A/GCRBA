using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace GCRBA.Models {
	public class Database {
		// this user object will be retrieved from where the user types in their data
		public User.ActionTypes InsertUser(User u) {
			try {
				//create a connection object
				SqlConnection cn = null;

				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_USER", cn);
				int intReturnValue = -1;

				// passing in the comand, name of what to mod, the value, the data type and where it's putting the 
				// data which is only pertnant to the first param (big int is an output param)
				SetParameter(ref cm, "@uid", u.UID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@user_id", u.Username, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				// once this line completes, it will return the return value from the db (if 1 then good)
				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: // new user created
						u.UID = (int)(long)cm.Parameters["@uid"].Value;
						return User.ActionTypes.InsertSuccessful;
					case -1:
						return User.ActionTypes.DuplicateEmail;
					case -2:
						return User.ActionTypes.DuplicateUserID;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// open database connection
		private bool GetDBConnection(ref SqlConnection SQLConn) {
			try {
				if (SQLConn == null) SQLConn = new SqlConnection();

				// check connection state
				if (SQLConn.State != ConnectionState.Open) {
					// no open connection, get connection string and try to open connection  
					SQLConn.ConnectionString = ConfigurationManager.AppSettings["AppDBConnect"];
					SQLConn.Open();
				}
				// connection successful 
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// close database connection 
		private bool CloseDBConnection(ref SqlConnection SQLConn) {
			try {
				// is connection closed?
				if (SQLConn.State != ConnectionState.Closed) {
					// no, so close it 
					SQLConn.Close();
					SQLConn.Dispose();
					SQLConn = null;
				}
				// connection closed successfully
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// log in user
		public User Login(User user) {
			try {
				// create instance of SqlConnection object 
				SqlConnection cn = new SqlConnection();

				// throw error if database connection unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// create instance of SqlDataAdapter object 
				SqlDataAdapter da = new SqlDataAdapter("LOGIN", cn);

				// create instance of DataSet
				DataSet ds;
				User newUser = null;

				// specify command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				// set parameters
				SetParameter(ref da, "@strUsername", user.Username, SqlDbType.NVarChar);
				SetParameter(ref da, "@strPassword", user.Password, SqlDbType.NVarChar);

				try {
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0) {
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.UID = Convert.ToInt16(dr["intUserID"]);
						newUser.FirstName = (string)dr["strFirstName"];
						newUser.LastName = (string)dr["strLastName"];
						newUser.Address = (string)dr["strAddress"];
						newUser.City = (string)dr["strCity"];
						newUser.State = (string)dr["strState"];
						newUser.Zip = (string)dr["strZip"];
						newUser.Phone = (string)dr["strPhone"];
						newUser.Email = (string)dr["strEmail"];
						newUser.Username = user.Username;
						newUser.Password = user.Password;
						newUser.isAdmin = Convert.ToInt16(dr["isAdmin"]);
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					CloseDBConnection(ref cn);
				}
				return newUser;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public void IsUserMember(User u)
		{
			try
			{
				// create instance of SqlConnection object 
				SqlConnection cn = new SqlConnection();

				// throw error if database connection unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// create instance of SqlDataAdapter object 
				SqlDataAdapter da = new SqlDataAdapter("VERIFY_MEMBER", cn);

				// create instance of DataSet
				DataSet ds;

				// specify command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				// set parameters
				SetParameter(ref da, "@intUserID", u.UID, SqlDbType.Int);

				if (u.UID == 0)
                {
					u.isMember = 0;
                } else
                {
					try
					{
						ds = new DataSet();
						da.Fill(ds);
						if (ds.Tables[0].Rows.Count > 0)
						{
							u.isMember = 1;
						}
						else
						{
							u.isMember = 0;
						}
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally
					{
						CloseDBConnection(ref cn);
					}
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public string GetMainBanner()
        {
			string banner = String.Empty;

			try
            {
				// declare variable to hold banner string 
				// create new instance of SqlConnection object 
				SqlConnection cn = new SqlConnection();

				// try to connect to DB 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// create instance of SqlDataAdapter object 
				SqlDataAdapter da = new SqlDataAdapter("GET_MAIN_BANNER", cn);

				// create instance of DataSet
				DataSet ds;

				// specify command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try
                {
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
                    {
						DataRow dr = ds.Tables[0].Rows[0];
						banner = (string)dr["strBanner"];
						return banner;
                    }
                }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
                {
					CloseDBConnection(ref cn);
                }
            }
			catch (Exception ex) { throw new Exception(ex.Message); }

			return banner;
        }

		public List<Company> GetCompanies()
        {
			try
            {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_COMPANY_INFO", cn);

				// create new instance of Company list 
				List<Company> companies = new List<Company>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try
                {
					da.Fill(ds);
                }
				catch (Exception ex)
                {
					throw new Exception(ex.Message);
                }
				finally
                {
					CloseDBConnection(ref cn);
                }
				if (ds.Tables[0].Rows.Count != 0)
                {
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
                    {
						// create new Company object
						Company c = new Company();

						// add values to CompanyID and Name properties 
						c.CompanyID = Convert.ToInt16(dr["intCompanyID"]);
						c.Name = (string)dr["strCompanyName"];

						// add Company object (c) to Company list (companies) 
						companies.Add(c);
                    }
                }
				// return list of companies 
				return companies;
            }
			catch (Exception ex) { throw new Exception(ex.Message); }
        }

		private int SetParameter(ref SqlCommand cm, string ParameterName, Object Value
			, SqlDbType ParameterType, int FieldSize = -1
			, ParameterDirection Direction = ParameterDirection.Input
			, Byte Precision = 0, Byte Scale = 0) {
			try {
				cm.CommandType = CommandType.StoredProcedure;
				if (FieldSize == -1)
					cm.Parameters.Add(ParameterName, ParameterType);
				else
					cm.Parameters.Add(ParameterName, ParameterType, FieldSize);

				if (Precision > 0) cm.Parameters[cm.Parameters.Count - 1].Precision = Precision;
				if (Scale > 0) cm.Parameters[cm.Parameters.Count - 1].Scale = Scale;

				cm.Parameters[cm.Parameters.Count - 1].Value = Value;
				cm.Parameters[cm.Parameters.Count - 1].Direction = Direction;

				return 0;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		private int SetParameter(ref SqlDataAdapter cm, string ParameterName, Object Value
			, SqlDbType ParameterType, int FieldSize = -1
			, ParameterDirection Direction = ParameterDirection.Input
			, Byte Precision = 0, Byte Scale = 0) {
			try {
				cm.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (FieldSize == -1)
					cm.SelectCommand.Parameters.Add(ParameterName, ParameterType);
				else
					cm.SelectCommand.Parameters.Add(ParameterName, ParameterType, FieldSize);

				if (Precision > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Precision = Precision;
				if (Scale > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Scale = Scale;

				cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Value = Value;
				cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Direction = Direction;

				return 0;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes UpdateUser(User u) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_USER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@uid", u.UID, SqlDbType.BigInt);
				SetParameter(ref cm, "@user_name", u.Username, SqlDbType.NVarChar);
				SetParameter(ref cm, "@password", u.Password, SqlDbType.NVarChar);
				SetParameter(ref cm, "@first_name", u.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@last_name", u.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@email", u.Email, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: //new updated
						return User.ActionTypes.UpdateSuccessful;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes InsertCompany(NewLocation loc) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_COMPANY", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", loc.lngCompanyID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strCompanyName", loc.LocationName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAbout", loc.Bio, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strBizYear", loc.BizYear, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: // new user created
						loc.lngCompanyID = (long)cm.Parameters["@intCompanyID"].Value;
						return NewLocation.ActionTypes.InsertSuccessful;
					default:
						return NewLocation.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes InsertLocation(NewLocation loc) {
			try {
				//Convert Phone Class to Concat String
				string PhoneNumber = loc.BusinessPhone.AreaCode + loc.BusinessPhone.Prefix + loc.BusinessPhone.Suffix;

				//ONLY DELETE intState!!!!!!!!!!!!!!!!!!
				loc.intState = 1;

				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", loc.lngLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intCompanyID", loc.lngCompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strAddress", loc.LocationName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", loc.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", loc.intState, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strZip", loc.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", PhoneNumber, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", loc.BusinessEmail, SqlDbType.NVarChar);
				

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: // new user created
						loc.lngLocationID = (long)cm.Parameters["@intLocationID"].Value;
						return NewLocation.ActionTypes.InsertSuccessful;
					default:
						return NewLocation.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes InsertSpecialties(NewLocation loc, List<Models.CategoryItem> categories) {
			try {
				int[] arrReturnValue = new int[] { 1 };
				List<int> ls = arrReturnValue.ToList();

				foreach (Models.CategoryItem item in categories) {
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_CATEGORYLOCATION", cn);

					int blnAvailable = 0;
					if (item.blnAvailable == true) blnAvailable = 1;

					SetParameter(ref cm, "@intCategoryLocationID", item.lngCategoryLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCategoryID", item.ItemID, SqlDbType.SmallInt);
					SetParameter(ref cm, "@intLocationID", loc.lngLocationID, SqlDbType.BigInt);
					SetParameter(ref cm, "@blnAvailable", blnAvailable, SqlDbType.Bit);


					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					item.lngCategoryLocationID = (long)cm.Parameters["@intCategoryLocationID"].Value;
					ls.Add((int)cm.Parameters["ReturnValue"].Value);
					CloseDBConnection(ref cn);
				}

				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return NewLocation.ActionTypes.Unknown;
					}
				}
				return NewLocation.ActionTypes.InsertSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}
		
		public NewLocation.ActionTypes InsertLocationHours(NewLocation loc, List<Models.Days> LocationHours) {
			try {
				int[] arrReturnValue = new int[] { 1 };
				List<int> ls = arrReturnValue.ToList();

				foreach (Models.Days item in LocationHours) {
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_LOCATIONHOURS", cn);

					if (item.strOpenTime != string.Empty) {
						item.dtOpenTime = Convert.ToDateTime(item.strOpenTime);
						item.strOpenTime = item.dtOpenTime.ToShortTimeString();
					}
					else item.strOpenTime = "Closed";

					if (item.strClosedTime != string.Empty) {
						item.dtClosedTime = Convert.ToDateTime(item.strClosedTime);
						item.strClosedTime = item.dtClosedTime.ToShortTimeString();
					}
					else item.strClosedTime = "Closed";

					SetParameter(ref cm, "@intLocationHoursID", item.intLocationHoursID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intLocationID", loc.lngLocationID, SqlDbType.BigInt);
					SetParameter(ref cm, "@intDayID", item.intDayID, SqlDbType.SmallInt);
					SetParameter(ref cm, "@strOpen", item.strOpenTime, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strClose", item.strClosedTime, SqlDbType.NVarChar);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					item.intLocationHoursID = (long)cm.Parameters["@intLocationHoursID"].Value;
					ls.Add((int)cm.Parameters["ReturnValue"].Value);
					CloseDBConnection(ref cn);
				}

				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return NewLocation.ActionTypes.Unknown;
					}
				}
				return NewLocation.ActionTypes.InsertSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		
		public NewLocation.ActionTypes InsertSocialMedia(NewLocation loc, List<Models.SocialMedia> socialMedias) {
			try {
				int[] arrReturnValue = new int[] { 1 };
				List<int> ls = arrReturnValue.ToList();

				foreach (Models.SocialMedia item in socialMedias) {
					if (item.blnAvailable == false) continue;
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_SOCIALMEDIA", cn);

					SetParameter(ref cm, "@intCompanySocialMediaID", item.intCompanySocialMediaID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@strSocialMediaLink", item.strSocialMediaLink, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intCompanyID", loc.lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@intSocialMediaID", item.intSocialMediaID, SqlDbType.SmallInt);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					item.intCompanySocialMediaID = (long)cm.Parameters["@intCompanySocialMediaID"].Value;
					ls.Add((int)cm.Parameters["ReturnValue"].Value);
					CloseDBConnection(ref cn);
				}

				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return NewLocation.ActionTypes.Unknown;
					}
				}
				return NewLocation.ActionTypes.InsertSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes InsertContactPerson(NewLocation loc, List<Models.ContactPerson> contacts) {
			try {
				int[] arrReturnValue = new int[] { 1 };
				List<int> ls = arrReturnValue.ToList();

				foreach (Models.ContactPerson item in contacts) {
					string name = item.strContactLastName + ", " + item.strContactFirstName;
					string phone = "(" + item.contactPhone.AreaCode + ") " + item.contactPhone.Prefix + "-" + item.contactPhone.Suffix ;


					//if (item.strContactFirstName == string.Empty || item.strContactLastName == string.Empty) continue;
					//if (item.contactPhone.AreaCode == string.Empty || item.contactPhone.Prefix == string.Empty) continue;

					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_CONTACTPERSON", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intContactPersonID", item.lngContactPersonID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@strContactName", name, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strContactPhone", phone, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strContactEmail", item.strContactEmail, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intLocationID", loc.lngLocationID, SqlDbType.BigInt);
					SetParameter(ref cm, "@intCompanyID", loc.lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@intContactPersonTypeID", item.intContactTypeID, SqlDbType.SmallInt);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
					CloseDBConnection(ref cn);
				}

				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return NewLocation.ActionTypes.Unknown;
					}
				}
				return NewLocation.ActionTypes.InsertSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes InsertWebsite(NewLocation loc, List<Models.Website> websites) {
			try {
				int[] arrReturnValue = new int[] { 1 };
				List<int> ls = arrReturnValue.ToList();

				foreach (Models.Website item in websites) {
					if (item.strURL == string.Empty) continue;
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_WEBSITE", cn);

					SetParameter(ref cm, "@intWebsiteID", item.intWebsiteID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCompanyID", loc.lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@strURL", item.strURL, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intWebsiteTypeID", item.intWebsiteTypeID, SqlDbType.SmallInt);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					item.intWebsiteID = (long)cm.Parameters["@intWebsiteID"].Value;
					ls.Add((int)cm.Parameters["ReturnValue"].Value);
					CloseDBConnection(ref cn);
				}

				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return NewLocation.ActionTypes.Unknown;
					}
				}
				return NewLocation.ActionTypes.InsertSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.NewLocation> GetLocations(List<Models.CategoryItem> categoryItems) {

			List<Models.NewLocation> locs = new List<Models.NewLocation>();
			foreach (Models.CategoryItem item in categoryItems) {
				try {
					DataSet ds = new DataSet();
					SqlConnection cn = new SqlConnection();
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlDataAdapter da = new SqlDataAdapter("SELECT_CATEGORYLOCATION", cn);

					da.SelectCommand.CommandType = CommandType.StoredProcedure;

					if (item.blnAvailable == true) SetParameter(ref da, "@intCategoryID", item.ItemID, SqlDbType.SmallInt);
					else continue;
					try {
						da.Fill(ds);
					}
					catch (Exception ex2) {
						throw new Exception(ex2.Message);
					}
					finally { CloseDBConnection(ref cn); }

					if (ds.Tables[0].Rows.Count != 0) {
						foreach (DataRow dr in ds.Tables[0].Rows) {
							NewLocation loc = new NewLocation();
							loc.lngLocationID = (long)dr["intLocationID"];
							loc.lngCompanyID = (long)dr["intCompanyID"];
							loc.LocationName = (string)dr["strCompanyName"];
							loc.StreetAddress = (string)dr["strAddress"];
							loc.City = (string)dr["strCity"];
							loc.State = (string)dr["strState"];
							loc.Zip = (string)dr["strZip"];
							loc.selectedGood = (string)dr["strCategory"];
							locs.Add(loc);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			}
			return locs;
		}

		public Models.NewLocation GetLandingLocation(long lngLocationID = 0) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION", cn);
				Models.NewLocation loc = new Models.NewLocation();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);
				try {
					da.Fill(ds);
				}
				catch (Exception ex2) {
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					foreach (DataRow dr in ds.Tables[0].Rows) {
						loc.lngLocationID = (long)dr["intLocationID"];
						loc.lngCompanyID = (long)dr["intCompanyID"];
						loc.LocationName = (string)dr["strCompanyName"];
						loc.StreetAddress = (string)dr["strAddress"];
						loc.City = (string)dr["strCity"];
						loc.State = (string)dr["strState"];
						loc.Zip = (string)dr["strZip"];
						loc.strFullPhone = (string)dr["strPhone"];
						loc.BusinessEmail = (string)dr["strEmail"];
					}
				}
				return loc;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
}