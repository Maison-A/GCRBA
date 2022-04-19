using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using GCRBA.ViewModels;
using System.Data.SqlTypes;

namespace GCRBA.Models
{

	public class Database
	{

		public List<Models.State> GetStates()
		{
			try
			{
				List<Models.State> lstStates = new List<Models.State>();
				try
				{
					DataSet ds = new DataSet();
					SqlConnection cn = new SqlConnection();
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlDataAdapter da = new SqlDataAdapter("SELECT_STATES", cn);

					da.SelectCommand.CommandType = CommandType.StoredProcedure;

					try
					{
						da.Fill(ds);
					}
					catch (Exception ex2)
					{
						throw new Exception(ex2.Message);
					}
					finally { CloseDBConnection(ref cn); }

					if (ds.Tables[0].Rows.Count != 0)
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							State state = new State();
							state.intStateID = (short)dr["intStateID"];
							state.strState = (string)dr["strState"];
							lstStates.Add(state);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }

				return lstStates;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public string GetState(int intStateID)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to db 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				// specift stored procedure to use
				SqlDataAdapter da = new SqlDataAdapter("GET_STATE", cn);

				// create variable to hold state 
				string strState = "";

				SetParameter(ref da, "@intStateID", intStateID, SqlDbType.BigInt);

				// set command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					strState = (string)dr["strState"];
				}
				return strState;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		public NewLocation.ActionTypes DeleteLocation(long lngLocationID)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@lngLocationID", lngLocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) return NewLocation.ActionTypes.DeleteSuccessful;
				return NewLocation.ActionTypes.Unknown;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public LocationList.ActionTypes DeleteLocations(Models.LocationList lstLocations) {
			int i = 0;

			try {
				foreach (GCRBA.Models.NewLocation item in lstLocations.lstLocations) {
					if (lstLocations.lstLocations[i] != null) {
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("DELETE_LOCATION", cn);
						int intReturnValue = -1;

						SetParameter(ref cm, "@lngLocationID", lstLocations.lstLocations[i].lngLocationID, SqlDbType.BigInt);
						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();

						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						CloseDBConnection(ref cn);

						if (intReturnValue != 1) return LocationList.ActionTypes.DeleteFailed;
						i += 1;
					}
				}
				return LocationList.ActionTypes.DeleteSuccessful;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// this user object will be retrieved from where the user types in their data
		public User.ActionTypes InsertUser(User u)
		{
			try
			{
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

				switch (intReturnValue)
				{
					case 1: // new user created
						u.UID = (int)(long)cm.Parameters["@uid"].Value;
						return User.ActionTypes.InsertSuccessful;
					case -1:
						return User.ActionTypes.DuplicateEmail;
					case -2:
						return User.ActionTypes.DuplicateUsername;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// open database connection
		private bool GetDBConnection(ref SqlConnection SQLConn)
		{
			try
			{
				if (SQLConn == null) SQLConn = new SqlConnection();

				// check connection state
				if (SQLConn.State != ConnectionState.Open)
				{
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
		private bool CloseDBConnection(ref SqlConnection SQLConn)
		{
			try
			{
				// is connection closed?
				if (SQLConn.State != ConnectionState.Closed)
				{
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

		public User.ActionTypes AddNewUser(User u)
		{
			try
			{
				// initialize return value 
				int intReturnValue = -1;

				// create instance of SqlConnection object 
				SqlConnection cn = null;

				// throw error if database connection unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure is being used 
				SqlCommand cm = new SqlCommand("INSERT_NEW_USER", cn);

				// set parameters
				SetParameter(ref cm, "@intNewUserID", u.UID, SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strFirstName", u.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strLastName", u.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAddress", u.Address, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", u.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", u.intState, SqlDbType.SmallInt);
				SetParameter(ref cm, "@strZip", u.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", u.Phone, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", u.Email, SqlDbType.NVarChar);
				
				SetParameter(ref cm, "@strUsername", u.Username, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPassword", u.Password, SqlDbType.NVarChar);
				SetParameter(ref cm, "@isAdmin", u.isAdmin, SqlDbType.Bit);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				// return user action type based on return value 
				switch (intReturnValue)
				{
					case 1:
						u.UID = Convert.ToInt16(cm.Parameters["@intNewUserID"].Value);
						return User.ActionTypes.InsertSuccessful;
					case -1:
						return User.ActionTypes.DuplicateEmail;
					case -2:
						return User.ActionTypes.DuplicateUsername;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		// log in user
		public User Login(User user)
		{
			try
			{
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

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
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
				finally
				{
					CloseDBConnection(ref cn);
				}
				
					return newUser;
				
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public User NonAdminLogin(User user)
		{
			try
			{
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

				if (IsUserAdmin(user) == false)
				{
					try
					{
						ds = new DataSet();
						da.Fill(ds);
						if (ds.Tables[0].Rows.Count > 0)
						{
							newUser = new User();
							DataRow dr = ds.Tables[0].Rows[0];
							newUser.UID = Convert.ToInt16(dr["intUserID"]);
							newUser.FirstName = (string)dr["strFirstName"];
							newUser.LastName = (string)dr["strLastName"];

							// db allows address, city, state, zip, and phone to be null in user table 
							// so must check if null here before adding to User object 
							// if not null, add to User, else skip these columns 
							if (dr["strAddress"].ToString() != SqlString.Null)
                            {
								newUser.Address = (string)dr["strAddress"];
                            }

							if (dr["strCity"].ToString() != SqlString.Null)
							{
								newUser.Address = (string)dr["strCity"];
							}

							if (dr["strState"].ToString() != SqlString.Null)
							{
								newUser.Address = (string)dr["strState"];
							}

							if (dr["strZip"].ToString() != SqlString.Null)
							{
								newUser.Address = (string)dr["strZip"];
							}

							if (dr["strPhone"].ToString() != SqlString.Null)
							{
								newUser.Address = (string)dr["strPhone"];
							}

							
							newUser.Email = (string)dr["strEmail"];
							newUser.Username = user.Username;
							newUser.Password = user.Password;
							newUser.isAdmin = Convert.ToInt16(dr["isAdmin"]);
						}
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally { CloseDBConnection(ref cn); }
					return newUser;
				}
				else
				{
					return newUser;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public User AdminLogin(User user)
		{
			try
			{
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

				if (IsUserAdmin(user) == true)
				{
					try
					{
						ds = new DataSet();
						da.Fill(ds);
						if (ds.Tables[0].Rows.Count > 0)
						{
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
					finally { CloseDBConnection(ref cn); }
					return newUser;
				}
				else
				{
					return newUser;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		
		public bool IsUserAdmin(User user)
		{
			try
			{
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

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.isAdmin = Convert.ToInt16(dr["isAdmin"]);
					}

					if (newUser == null || newUser.isAdmin == 0)
					{
						return false;
					}
					else
					{
						return true;
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }
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
				}
				else
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

		public Company GetCompanyInfo(EditCompaniesViewModel vm)
		{
			try
			{
				// create new instance of SqlConnection object 
				SqlConnection cn = new SqlConnection();

				// try to connect to DB 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// create instance of SqlDataAdapter object 
				SqlDataAdapter da = new SqlDataAdapter("GET_SPECIFIC_COMPANY", cn);

				// create instance of DataSet
				DataSet ds;

				// specify command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@intCompanyID", vm.CurrentCompany.CompanyID, SqlDbType.BigInt);

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						DataRow dr = ds.Tables[0].Rows[0];
						vm.CurrentCompany.Name = (string)dr["strCompanyName"];
						vm.CurrentCompany.About = (string)dr["strAbout"];
						vm.CurrentCompany.Year = (string)dr["strBizYear"];
						return vm.CurrentCompany;
					}
					return vm.CurrentCompany;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }
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

		public LocationList.ActionTypes InsertLocations(LocationList locList) {
			int i = 0;
			do {
				try {
					//Convert Phone Class to Concat String
					string PhoneNumber = locList.lstLocations[i].BusinessPhone.AreaCode + locList.lstLocations[i].BusinessPhone.Prefix + locList.lstLocations[i].BusinessPhone.Suffix;

					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@strAddress", locList.lstLocations[i].LocationName, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strCity", locList.lstLocations[i].City, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intStateID", locList.lstLocations[i].intState, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strZip", locList.lstLocations[i].Zip, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strPhone", PhoneNumber, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strEmail", locList.lstLocations[i].BusinessEmail, SqlDbType.NVarChar);


					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					CloseDBConnection(ref cn);
					intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
					if (intReturnValue != 1) return Models.LocationList.ActionTypes.LocationExists;
					locList.lstLocations[i].lngLocationID = (long)cm.Parameters["@intLocationID"].Value;
					/*
					switch (intReturnValue) {
						case 1: // new user created
							locList.lstLocations[0].lngLocationID = (long)cm.Parameters["@intLocationID"].Value;
							return LocationList.ActionTypes.InsertSuccessful;
						default:
							return LocationList.ActionTypes.Unknown;
					}
					*/
					i += 1;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }

			} while (locList.lstLocations[i] != null);

			return LocationList.ActionTypes.InsertSuccessful;
		}

		public List<MainBanner> GetMainBanners()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_ALL_MAIN_BANNERS", cn);

				// create new list object with type string  
				List<MainBanner> banners = new List<MainBanner>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new MainBanner object
						MainBanner mb = new MainBanner();

						// add values to BannerID and Banner
						mb.BannerID = Convert.ToInt16(dr["intMainBannerID"]);
						mb.Banner = (string)dr["strBanner"];

						// add MainBanner object (mb) to MainBanner list (banners)
						banners.Add(mb);
					}
				}
				return banners;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Location> GetLocations(EditCompaniesViewModel vm)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_LOCATIONS", cn);

				SetParameter(ref da, "@intCompanyID", vm.CurrentCompany.CompanyID, SqlDbType.BigInt);

				// create new list object with type string  
				List<Location> locations = new List<Location>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create location object 
						Location l = new Location();

						// add values to LocationID and Location string
						l.LocationID = Convert.ToInt16(dr["intLocationID"]);
						l.Address = (string)dr["strAddress"];
						l.City = (string)dr["strCity"];
						l.State = (string)dr["strState"];
						l.Zip = (string)dr["strZip"];
						l.Phone = (string)dr["strPhone"];
						l.Email = (string)dr["strEmail"];

						// add location object to list of location objects 
						locations.Add(l);
					}
				}
				return locations;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<CategoryItem> GetNotCategories(EditCompaniesViewModel vm)
        {
			vm.Categories = GetCategories(vm, "GET_NOT_CATEGORIES");

			return vm.Categories;
        }

		public List<CategoryItem> GetCurrentCategories(EditCompaniesViewModel vm)
        {
			vm.Categories = GetCategories(vm, "GET_CURRENT_CATEGORIES");

			return vm.Categories;
		}

		public List<CategoryItem> GetCategories(EditCompaniesViewModel vm, string sproc)
        {
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter(sproc, cn);

				SetParameter(ref da, "@intLocationID", vm.CurrentLocation.LocationID, SqlDbType.BigInt);

				// create new list object with type string  
				List<CategoryItem> categories = new List<CategoryItem>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create category object
						CategoryItem c = new CategoryItem();

						// add values to category object
						c.ItemID = Convert.ToInt16(dr["intCategoryID"]);
						c.ItemDesc = (string)dr["strCategory"];

						// add category object to list of categorie 
						categories.Add(c);
					}
				}
				return categories;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<ContactPerson> GetContactsByCompany(EditCompaniesViewModel vm)
        {
			try
            {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_CONTACTS_BY_COMPANY", cn);

				SetParameter(ref da, "@intCompanyID", vm.CurrentCompany.CompanyID, SqlDbType.BigInt);

				// create new list object with type string  
				List<ContactPerson> contacts = new List<ContactPerson>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
                {
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
                    {
						// create ContactPerson object
						ContactPerson c = new ContactPerson();

						// add values to ContactPerson object 
						c.lngContactPersonID = Convert.ToInt16(dr["intContactPersonID"]);
						c.strFullName = (string)dr["strContactName"];
						c.intContactTypeID = Convert.ToInt16(dr["intContactPersonTypeID"]);
						c.strContactPersonType = (string)dr["strContactPersonType"];

						if (dr["strContactPhone"].ToString() != SqlString.Null)
                        {
							c.strFullPhone = (string)dr["strContactPhone"];
                        }

						if (dr["strContactEmail"].ToString() != SqlString.Null)
						{
							c.strContactEmail = (string)dr["strContactEmail"];
						}

						if (dr["intLocationID"].ToString() != SqlString.Null)
                        {
							c.intLocationID = Convert.ToInt16(dr["intLocationID"]);
                        }

						// add contactperson object to list of contacts
						contacts.Add(c);
					}
                }
				return contacts;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public List<Location> GetLocationsNotContact(EditCompaniesViewModel vm)
        {
			try
            {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_LOCATIONS_NOT_CONTACT", cn);

				SetParameter(ref da, "@intContactPersonID", vm.ContactPerson.lngContactPersonID, SqlDbType.BigInt);
				SetParameter(ref da, "@intCompanyID", vm.CurrentCompany.CompanyID, SqlDbType.BigInt);

				// create new list object with type string  
				List<Location> locations = new List<Location>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
                {
					foreach (DataRow dr in ds.Tables[0].Rows)
                    {
						// create location object
						Location l = new Location();

						// add values to Location object from db 
						l.LocationID = Convert.ToInt16(dr["intLocationID"]);
						l.Address = (string)dr["strAddress"];
						l.City = (string)dr["strCity"];
						l.State = (string)dr["strState"];
						l.Zip = (string)dr["strZip"];

						// add to list of locations 
						locations.Add(l);
                    }
                }
				return locations;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
        }

		public List<NewLocation> GetMemberLocations(Models.User user) {
			List<NewLocation> lstMemLocations = new List<NewLocation>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_USERLOCATION_ASSOCIATION", cn);
				

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (user.UID > 0) SetParameter(ref da, "@intUserID", user.UID, SqlDbType.Int);
				try {
					da.Fill(ds);
				}
				catch (Exception ex2) {
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					foreach (DataRow dr in ds.Tables[0].Rows) {
						Models.NewLocation loc = new Models.NewLocation();
						loc.lngLocationID = (long)dr["intLocationID"];
						loc.lngCompanyID = (long)dr["intCompanyID"];
						loc.LocationName = (string)dr["strCompanyName"];
						loc.StreetAddress = (string)dr["strAddress"];
						loc.City = (string)dr["strCity"];
						loc.intState = (short)dr["intStateID"];
						loc.Zip = (string)dr["strZip"];
						loc.strFullPhone = (string)dr["strPhone"];
						loc.BusinessEmail = (string)dr["strEmail"];
						lstMemLocations.Add(loc);
					}
				}
				return lstMemLocations;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
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

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

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

		public bool CheckMemberStatus(long lngLocationID = 0) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("CHECK_IF_MEMBERLOCATION", cn);

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }
				int result = 0;
				if (ds.Tables[0].Rows.Count != 0) {
					foreach (DataRow dr in ds.Tables[0].Rows) {
						result = (short)dr["intMemberLevelID"];
					}
					if (result == 2) {
						return true;
					}
					else {
						return false;
					}
				}
				else return false;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		
		}

		public Company.ActionTypes DeleteCompany(Company c)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_COMPANY", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1:
						return Company.ActionTypes.DeleteSuccessful;
					default:
						return Company.ActionTypes.Unknown;

				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public CategoryItem.ActionTypes DeleteCategories(EditCompaniesViewModel vm)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_CATEGORY_FROM_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", vm.CurrentLocation.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intCategoryID", vm.Category.ItemID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1:
						return CategoryItem.ActionTypes.DeleteSuccessful;
					default:
						return CategoryItem.ActionTypes.Unknown;

				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
        }

		public bool InsertHomepageBanner()
		{
			MainBanner mb = new MainBanner();

			try
			{
				SqlConnection cn = null; // inside System.Data.SqlClient
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_NEW_MAIN_BANNER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intMainBannerID", mb.BannerID, SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strBanner", mb.Banner, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					mb.BannerID = (int)cm.Parameters["@intMainBannerID"].Value;
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}


		private int SetParameter(ref SqlCommand cm, string ParameterName, Object Value
			, SqlDbType ParameterType, int FieldSize = -1
			, ParameterDirection Direction = ParameterDirection.Input
			, Byte Precision = 0, Byte Scale = 0)
		{
			try
			{
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
			, Byte Precision = 0, Byte Scale = 0)
		{
			try
			{
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

		public User.ActionTypes UpdateUser(User u)
		{
			try
			{
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

				switch (intReturnValue)
				{
					case 1: //new updated
						return User.ActionTypes.UpdateSuccessful;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Company.ActionTypes InsertNewCompany(Company c)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_COMPANY", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strCompanyName", c.Name, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAbout", c.About, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strBizYear", c.Year, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case -1:
						return Company.ActionTypes.DuplicateName;
					case 1: // new user created
						c.CompanyID = Convert.ToInt16(cm.Parameters["@intCompanyID"].Value);
						return Company.ActionTypes.InsertSuccessful;
					default:
						return Company.ActionTypes.Unknown;
				}

			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public CategoryItem.ActionTypes InsertCategories(EditCompaniesViewModel vm)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_CATEGORYLOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCategoryLocationID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intCategoryID", vm.Category.ItemID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intLocationID", vm.CurrentLocation.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
                {
					return CategoryItem.ActionTypes.InsertSuccessful;
                }
				else
                {
					return CategoryItem.ActionTypes.Unknown;
                }

			}
			catch (Exception ex) { throw new Exception(ex.Message); }
        }

		public LocationList.ActionTypes InsertCompany(LocationList locList)
		{
			int i = 0;
			do
			{
				try
				{
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_COMPANY", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@strCompanyName", locList.lstLocations[i].CompanyName, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strAbout", locList.lstLocations[i].Bio, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strBizYear", locList.lstLocations[i].BizYear, SqlDbType.NVarChar);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();

					
					CloseDBConnection(ref cn);
					intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
					if (intReturnValue == -1) return Models.LocationList.ActionTypes.CompanyNameExists;
					locList.lstLocations[i].lngCompanyID = (long)cm.Parameters["@intCompanyID"].Value;
					i += 1;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			} while (locList.lstLocations[i] != null);
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertLocation(LocationList locList)
		{
			int i = 0;
			do
			{
				try
				{
					//Convert Phone Class to Concat String
					string PhoneNumber = locList.lstLocations[i].BusinessPhone.AreaCode + locList.lstLocations[i].BusinessPhone.Prefix + locList.lstLocations[i].BusinessPhone.Suffix;

					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@strAddress", locList.lstLocations[i].LocationName, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strCity", locList.lstLocations[i].City, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intStateID", locList.lstLocations[i].intState, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strZip", locList.lstLocations[i].Zip, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strPhone", PhoneNumber, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strEmail", locList.lstLocations[i].BusinessEmail, SqlDbType.NVarChar);

					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

					cm.ExecuteReader();
					
					CloseDBConnection(ref cn);
					intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
					if (intReturnValue != 1) return LocationList.ActionTypes.LocationExists;
					locList.lstLocations[i].lngLocationID = (long)cm.Parameters["@intLocationID"].Value;
					i += 1;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }

			} while (locList.lstLocations[i] != null);

			return LocationList.ActionTypes.InsertSuccessful;
		}

		public NewLocation.ActionTypes AddNewLocation(EditCompaniesViewModel vm)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intCompanyID", vm.CurrentCompany.CompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "strAddress", vm.NewLocation.StreetAddress, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", vm.NewLocation.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", vm.NewLocation.intState, SqlDbType.SmallInt);
				SetParameter(ref cm, "@strZip", vm.NewLocation.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", vm.NewLocation.strFullPhone, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", vm.NewLocation.BusinessEmail, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
                {
					return NewLocation.ActionTypes.InsertSuccessful;
                } else
                {
					return NewLocation.ActionTypes.Unknown;
                }
            }
			catch (Exception ex) { throw new Exception(ex.Message); }
        }

		public LocationList.ActionTypes InsertSpecialties(LocationList locList, List<Models.CategoryItem>[] categories)
		{
			int intReturnValue = 0;
			int i = 0;
			do
			{
				try
				{
					foreach (Models.CategoryItem item in categories[i])
					{
						if (item.blnAvailable == true)
						{
							SqlConnection cn = null;
							if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
							SqlCommand cm = new SqlCommand("INSERT_CATEGORYLOCATION", cn);

							SetParameter(ref cm, "@intCategoryLocationID", item.lngCategoryLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
							SetParameter(ref cm, "@intCategoryID", item.ItemID, SqlDbType.SmallInt);
							SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt);

							SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

							cm.ExecuteReader();
							
							CloseDBConnection(ref cn);
							intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
							if (intReturnValue != 1) return Models.LocationList.ActionTypes.CategoryLocationExists;

							item.lngCategoryLocationID = (long)cm.Parameters["@intCategoryLocationID"].Value;
						}
					}
					i += 1;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			} while (locList.lstLocations[i] != null);
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertLocationHours(LocationList locList, List<Models.Days>[] LocationHours)
		{
			int i = 0;
			int intReturnValue = -1;
			try { 
				do {
					foreach (Models.Days item in LocationHours[i]) {

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
						SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt);
						SetParameter(ref cm, "@intDayID", item.intDayID, SqlDbType.SmallInt);
						SetParameter(ref cm, "@strOpen", item.strOpenTime, SqlDbType.NVarChar);
						SetParameter(ref cm, "@strClose", item.strClosedTime, SqlDbType.NVarChar);

						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();
						
						CloseDBConnection(ref cn);
						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						if (intReturnValue != 1) return Models.LocationList.ActionTypes.LocationHourExists;

						item.intLocationHoursID = (long)cm.Parameters["@intLocationHoursID"].Value;
					}
					i += 1;
				}
				while (LocationHours[i] != null);

				/*
				arrReturnValue = ls.ToArray();
				foreach (int item in arrReturnValue) {
					switch (item) {
						case 1: // new user created
							break;
						default:
							return LocationList.ActionTypes.Unknown;
					}
				}
				return LocationList.ActionTypes.InsertSuccessful;
				*/
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertSocialMedia(LocationList locList, List<Models.SocialMedia>[] socialMedias)
		{
			int intReturnValue = 0;
			int i = 0;
			do
			{
				try
				{

					foreach (Models.SocialMedia item in socialMedias[i])
					{
						if (item.blnAvailable == false) continue;
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_SOCIALMEDIA", cn);

						SetParameter(ref cm, "@intCompanySocialMediaID", item.intCompanySocialMediaID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
						SetParameter(ref cm, "@strSocialMediaLink", item.strSocialMediaLink, SqlDbType.NVarChar);
						SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
						SetParameter(ref cm, "@intSocialMediaID", item.intSocialMediaID, SqlDbType.SmallInt);

						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();

						CloseDBConnection(ref cn);
						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						if (intReturnValue != 1) return Models.LocationList.ActionTypes.SocialMediaExists;

						item.intCompanySocialMediaID = (long)cm.Parameters["@intCompanySocialMediaID"].Value;
					}
					i += 1;
					/*
					arrReturnValue = ls.ToArray();
					foreach (int item in arrReturnValue) {
						switch (item) {
							case 1: // new user created
								break;
							default:
								return LocationList.ActionTypes.Unknown;
						}
					}
					return LocationList.ActionTypes.InsertSuccessful;
					*/
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			} while (locList.lstLocations[i] != null);
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertContactPerson(LocationList locList, List<Models.ContactPerson>[] contacts)
		{
			int i = 0;
			do
			{
				try
				{
					foreach (Models.ContactPerson item in contacts[i])
					{
						string name = item.strContactLastName + ", " + item.strContactFirstName;
						string phone = "(" + item.contactPhone.AreaCode + ") " + item.contactPhone.Prefix + "-" + item.contactPhone.Suffix;


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
						SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt);
						SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
						SetParameter(ref cm, "@intContactPersonTypeID", item.intContactTypeID, SqlDbType.SmallInt);

						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();
						
						CloseDBConnection(ref cn);
						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						if (intReturnValue != 1) return Models.LocationList.ActionTypes.ContactPersonExists;
					}
					i += 1;
					/*
					arrReturnValue = ls.ToArray();
					foreach (int item in arrReturnValue) {
						switch (item) {
							case 1: // new user created
								break;
							default:
								return LocationList.ActionTypes.Unknown;
						}
					}
					return LocationList.ActionTypes.InsertSuccessful;
					*/
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			} while (locList.lstLocations[i] != null);
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertWebsite(LocationList locList, List<Models.Website>[] websites)
		{
			int i = 0;
			int intReturnValue = 0;
			do
			{
				try
				{
					foreach (Models.Website item in websites[i])
					{
						if (item.strURL == string.Empty || item.strURL == null) continue;
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_WEBSITE", cn);

						SetParameter(ref cm, "@intWebsiteID", item.intWebsiteID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
						SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
						SetParameter(ref cm, "@strURL", item.strURL, SqlDbType.NVarChar);
						SetParameter(ref cm, "@intWebsiteTypeID", item.intWebsiteTypeID, SqlDbType.SmallInt);

						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();
						
						CloseDBConnection(ref cn);
						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						if (intReturnValue != 1) return Models.LocationList.ActionTypes.WebpageURLExists;
						item.intWebsiteID = (long)cm.Parameters["@intWebsiteID"].Value;
						
						

					}
					i += 1;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			} while (locList.lstLocations[i] != null);
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public List<Models.NewLocation> GetLocations(List<Models.CategoryItem> categoryItems)
		{

			List<Models.NewLocation> locs = new List<Models.NewLocation>();
			foreach (Models.CategoryItem item in categoryItems)
			{
				try
				{
					DataSet ds = new DataSet();
					SqlConnection cn = new SqlConnection();
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_BYCATEGORY", cn);

					da.SelectCommand.CommandType = CommandType.StoredProcedure;

					if (item.blnAvailable == true) SetParameter(ref da, "@intCategoryID", item.ItemID, SqlDbType.SmallInt);
					else continue;
					try
					{
						da.Fill(ds);
					}
					catch (Exception ex2)
					{
						throw new Exception(ex2.Message);
					}
					finally { CloseDBConnection(ref cn); }

					if (ds.Tables[0].Rows.Count != 0)
					{
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
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

		public Models.NewLocation GetLandingLocation(long lngLocationID = 0)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION", cn);
				Models.NewLocation loc = new Models.NewLocation();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);
				try
				{
					da.Fill(ds);
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
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

		public List<Models.CategoryItem> GetLandingCategories(long lngLocationID = 0)
		{
			List<Models.CategoryItem> lstCategories = new List<CategoryItem>();
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_ALLCATEGORY_FORLOCATION", cn);

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);
				try
				{
					da.Fill(ds);
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Models.CategoryItem item = new CategoryItem();
						item.ItemID = (short)dr["intCategoryID"];
						item.ItemDesc = (string)dr["strCategory"];
						item.blnAvailable = true;
						lstCategories.Add(item);
					}
				}
				return lstCategories;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.SaleSpecial> GetLandingSpecials(long lngLocationID = 0)
		{
			List<Models.SaleSpecial> lstSpecials = new List<SaleSpecial>();
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_SPECIALS", cn);

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);
				try
				{
					da.Fill(ds);
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Models.SaleSpecial item = new SaleSpecial();
						item.strDescription = (string)dr["strDescription"];
						item.monPrice = (decimal)dr["monPrice"];
						item.dtmStart = (DateTime)dr["dtmStart"];
						item.dtmEnd = (DateTime)dr["dtmEnd"];
						lstSpecials.Add(item);
					}
				}
				return lstSpecials;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.Awards> GetLandingAwards(long lngLocationID = 0)
		{
			List<Models.Awards> lstAwards = new List<Awards>();
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_AWARDS", cn);

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (lngLocationID > 0) SetParameter(ref da, "@intLocationID", lngLocationID, SqlDbType.BigInt);
				try
				{
					da.Fill(ds);
				}
				catch (Exception ex2)
				{
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Models.Awards item = new Awards();
						item.strFrom = (string)dr["strFrom"];
						item.strAward = (string)dr["strAward"];
						lstAwards.Add(item);
					}
				}
				return lstAwards;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.ContactPerson> GetLandingContacts(long lngLocationID = 0) {
			List<Models.ContactPerson> lstContactPerson = new List<ContactPerson>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_CONTACTS", cn);

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
						Models.ContactPerson item = new ContactPerson();
						item.strFullName = (string)dr["strContactName"];
						//item.strFullPhone = (string)dr["strContactPhone"];
						item.strContactEmail = (string)dr["strContactEmail"];
						item.intContactTypeID = (short)dr["intContactPersonTypeID"];
						lstContactPerson.Add(item);
					}
				}
				return lstContactPerson;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.SocialMedia> GetLandingSocialMedia(long lngLocationID = 0) {
			List<Models.SocialMedia> lstSocialMedia = new List<SocialMedia>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_SOCIALMEDIA", cn);

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
						Models.SocialMedia item = new SocialMedia();
						item.strSocialMediaLink = (string)dr["strSocialMediaLink"];
						item.strPlatform = (string)dr["strPlatform"];
						item.intCompanyID = (long)dr["intCompanyID"];
						lstSocialMedia.Add(item);
					}
				}
				return lstSocialMedia;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool InsertNewMainBanner(AdminBannerViewModel vm)
		{

			try
			{
				SqlConnection cn = null; // inside System.Data.SqlClient
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_NEW_MAIN_BANNER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intNewBannerID", "null", SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strBanner", vm.MainBanner.Banner, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					vm.MainBanner.BannerID = Convert.ToInt16(cm.Parameters["@intNewBannerID"].Value);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.Website> GetLandingWebsite(long lngLocationID = 0) {
			List<Models.Website> lstWebites = new List<Website>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_WEBSITE", cn);

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
						Models.Website item = new Website();
						item.strWebsiteType = (string)dr["strWebsiteType"];
						item.intWebsiteTypeID = (short)dr["intWebsiteTypeID"];
						item.strURL = (string)dr["strURL"];
						lstWebites.Add(item);
					}
				}
				return lstWebites;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
}