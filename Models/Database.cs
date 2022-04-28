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

		public Company GetCompanyByMember(ProfileViewModel vm)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to db 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				// specift stored procedure to use
				SqlDataAdapter da = new SqlDataAdapter("GET_COMPANY_BY_MEMBER", cn);

				SetParameter(ref da, "@intMemberID", vm.User.UID, SqlDbType.BigInt);

				// set command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					vm.Company.CompanyID = Convert.ToInt16(dr["intCompanyID"]);
					vm.Company.Name = (string)dr["strCompanyName"];
					vm.Company.About = (string)dr["strAbout"];
					vm.Company.Year = (string)dr["strBizYear"];
				}
				return vm.Company;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes DeleteLocation(long lngLocationID, long lngCompanyID)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@lngLocationID", lngLocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@lngCompanyID", lngCompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) return NewLocation.ActionTypes.DeleteSuccessful;
				return NewLocation.ActionTypes.Unknown;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Location.ActionTypes MemberDeleteLocation(Location l)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETELOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) return Location.ActionTypes.DeleteSuccessful;
				return Location.ActionTypes.Unknown;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes DeleteTempLocation(long lngLocationID, long lngCompanyID) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_TEMP_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@lngLocationID", lngLocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@lngCompanyID", lngCompanyID, SqlDbType.BigInt);
				
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) return NewLocation.ActionTypes.DeleteSuccessful;
				return NewLocation.ActionTypes.Unknown;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public NewLocation.ActionTypes DeleteAdminRequest(short intAdminRequestID) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_ADMIN_REQUEST", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intAdminRequestID", intAdminRequestID, SqlDbType.SmallInt);
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
							newUser.Email = (string)dr["strEmail"];

							// db allows address, city, state, zip, and phone to be null in user table 
							// so must check if null here before adding to User object 
							// if not null, add to User, else skip these columns 

							if (!DBNull.Value.Equals(dr["strAddress"]))
							{
								newUser.Address = (string)dr["strAddress"];
							}

							if (!DBNull.Value.Equals(dr["strCity"]))
							{
								newUser.City = (string)dr["strCity"];
							}

							if (!DBNull.Value.Equals(dr["strState"]))
							{
								newUser.intState = Convert.ToInt16(dr["intStateID"]);
								newUser.State = (string)dr["strState"];
							}

							if (!DBNull.Value.Equals(dr["strZip"]))
							{
								newUser.Zip = (string)dr["strZip"];
							}

							if (!DBNull.Value.Equals(dr["strPhone"]))
							{
								newUser.Phone = (string)dr["strPhone"];
							}

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

		public short GetMemberID(short UID) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("VERIFY_MEMBER", cn);

				short intMemberID = 0;

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@intUserID", UID, SqlDbType.SmallInt);

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows) {
						// create new MainBanner object
						intMemberID = (short)dr["intMemberID"];
					}
				}
				return intMemberID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Company GetCompanyInfo(AdminVM vm)
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

				SetParameter(ref da, "@intCompanyID", vm.Company.CompanyID, SqlDbType.BigInt);

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						DataRow dr = ds.Tables[0].Rows[0];
						vm.Company.Name = (string)dr["strCompanyName"];
						vm.Company.About = (string)dr["strAbout"];
						vm.Company.Year = (string)dr["strBizYear"];
						return vm.Company;
					}
					return vm.Company;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int GetTotalRequests()
		{
			try
			{
				// create new SqlConnection object
				SqlConnection cn = new SqlConnection();

				// try to connect to db 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				// create new SqlDataAdapter object
				SqlDataAdapter da = new SqlDataAdapter("GET_TOTAL_REQUESTS", cn);

				// create instance of DataSet
				DataSet ds;

				// specify command type 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				// create variable to hold total requests 
				int total = 0;

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						DataRow dr = ds.Tables[0].Rows[0];
						total = Convert.ToInt16(dr["TotalRequests"]);
					}
					return total;
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

					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@strAddress", locList.lstLocations[i].StreetAddress, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strCity", locList.lstLocations[i].City, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intStateID", locList.lstLocations[i].intState, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strZip", locList.lstLocations[i].Zip, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strPhone", locList.lstLocations[i].strFullPhone, SqlDbType.NVarChar);
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

		public List<Location> GetLocations(Company c)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_LOCATIONS", cn);

				SetParameter(ref da, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);

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
						l.intState = Convert.ToInt16(dr["intStateID"]);
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

		public Location GetLocation(Location l)
		{
			try
			{
				// create new instance of SqlConnection object 
				SqlConnection cn = new SqlConnection();

				// try to connect to DB 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// create instance of SqlDataAdapter object 
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION", cn);

				// create instance of DataSet
				DataSet ds;

				// specify command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@intLocationID", l.LocationID, SqlDbType.BigInt);

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						DataRow dr = ds.Tables[0].Rows[0];
						l.Address = (string)dr["strAddress"];
						l.City = (string)dr["strCity"];
						l.intState = Convert.ToInt16(dr["intStateID"]);
						l.Zip = (string)dr["strZip"];
						l.Phone = (string)dr["strPhone"];
						l.Email = (string)dr["strEmail"];
					}
					return l;
				} 
				catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<CategoryItem> GetNotCategories(List<CategoryItem> categories, Location l)
        {
			categories = GetCategories(l, "GET_NOT_CATEGORIES");

			return categories;
        }

		public List<CategoryItem> GetCurrentCategories(List<CategoryItem> categories, Location l)
        {
			categories = GetCategories(l, "GET_CURRENT_CATEGORIES");

			return categories;
		}

		public List<CategoryItem> GetCategories(Location l, string sproc)
        {
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter(sproc, cn);

				SetParameter(ref da, "@intLocationID", l.LocationID, SqlDbType.BigInt);

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

		public List<ContactPerson> GetContactsByCompany(AdminVM vm)
        {
			try
            {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_CONTACTS_BY_COMPANY", cn);

				SetParameter(ref da, "@intCompanyID", vm.Company.CompanyID, SqlDbType.BigInt);

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

		public List<Location> GetLocationsNotContact(AdminVM vm)
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
				SetParameter(ref da, "@intCompanyID", vm.Company.CompanyID, SqlDbType.BigInt);

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

		public List<Website> GetWebsiteTypes()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_WEBSITE_TYPES", cn);

				// create new instance of Company list 
				List<Website> types = new List<Website>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new Company object
						Website w = new Website();

						// add values to CompanyID and Name properties 
						w.intWebsiteTypeID = Convert.ToInt16(dr["intWebsiteTypeID"]);
						w.strWebsiteType = (string)dr["strWebsiteType"];

						// add Company object (c) to Company list (companies) 
						types.Add(w);
					}
				}
				// return list of companies 
				return types;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<SocialMedia> GetSocialMediaTypes()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_SOCIALMEDIA_TYPES", cn);

				// create new instance of Company list 
				List<SocialMedia> types = new List<SocialMedia>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new Company object
						SocialMedia s = new SocialMedia();

						// add values to CompanyID and Name properties 
						s.intSocialMediaID = Convert.ToInt16(dr["intSocialMediaID"]);
						s.strPlatform = (string)dr["strPlatform"];

						// add Company object (c) to Company list (companies) 
						types.Add(s);
					}
				}
				// return list of companies 
				return types;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<MemberRequest> GetMembershipRequests()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_MEMBERSHIP_REQUESTS", cn);

				// create new instance of Company list 
				List<MemberRequest> requests = new List<MemberRequest>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new MemberRequest object
						MemberRequest request = new MemberRequest();

						// add values 
						request.FirstName = (string)dr["strFirstName"];
						request.LastName = (string)dr["strLastName"];
						request.MemberID = Convert.ToInt16(dr["intMemberID"]);

						// add to list of membership requests 
						requests.Add(request);
					}
				}
				return requests;

			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public MemberRequest GetMemberInfo(AdminVM vm)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// create MemberRequest object
				MemberRequest m = new MemberRequest();

				// try to connect to db 
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				// specift stored procedure to use
				SqlDataAdapter da = new SqlDataAdapter("GET_MEMBER_INFO", cn);

				SetParameter(ref da, "@intMemberID", vm.MemberRequest.MemberID, SqlDbType.SmallInt);

				// set command type as stored procedure 
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					m.UserID = Convert.ToInt16(dr["intUserID"]);
					m.FirstName = (string)dr["strFirstName"];
					m.LastName = (string)dr["strLastName"];

					if (dr["strEmail"].ToString() == SqlString.Null)
					{
						m.Email = "";
					}
					else
					{
						m.Email = (string)dr["strEmail"];
					}

					if (dr["strPhone"].ToString() == SqlString.Null)
					{
						m.Phone = "";
					}
					else
					{
						m.Phone = (string)dr["strPhone"];
					}

					m.MemberID = Convert.ToInt16(dr["intMemberID"]);
					m.MemberLevel = (string)dr["strMemberLevel"];
					m.PaymentType = (string)dr["strPaymentType"];
					m.PaymentStatus = (string)dr["strStatus"];

				}
				return m;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Notification> GetUserNotifications(User u)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_USER_NOTIFICATIONS", cn);

				SetParameter(ref da, "@intUserID", u.UID, SqlDbType.SmallInt);

				// create new instance of Company list 
				List<Notification> messages = new List<Notification>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new Notification object 
						Notification message = new Notification();

						// add values 
						message.NotificationID = Convert.ToInt16(dr["intUserNotificationID"]);
						message.Message = (string)dr["strNotification"];
						message.NotificationStatusID = Convert.ToInt16(dr["intNotificationStatusID"]);
						message.NotificationStatus = (string)dr["strNotificationStatus"];

						// add to list 
						messages.Add(message);
					}
				}
				return messages;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<AdminNotification> GetAdminNotifications()
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_ADMIN_NOTIFICATIONS", cn);

				// create new instance of Company list 
				List<AdminNotification> messages = new List<AdminNotification>();

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
						// create new Notification object 
						AdminNotification message = new AdminNotification();

						// add values 
						message.NotificationID = Convert.ToInt16(dr["intAdminNotificationID"]);
						message.UserID = Convert.ToInt16(dr["intUserID"]);
						message.UserFirstName = (string)dr["strFirstName"];
						message.UserLastName = (string)dr["strLastName"];
						message.EditedColumnID = Convert.ToInt16(dr["intEditedColumnID"]);
						message.EditedTableID = Convert.ToInt16(dr["intEditedTableID"]);
						message.EditedColumn = (string)dr["strColumnName"];
						message.EditedTable = (string)dr["strTableName"];
						message.PreviousVersion = (string)dr["strPreviousVersion"];
						message.NewVersion = (string)dr["strNewVersion"];
						message.NotificationStatusID = Convert.ToInt16(dr["intNotificationStatusID"]);

						message.Message = "User: " + message.UserFirstName + " " + message.UserLastName + " | Edited Column: " + message.EditedColumn + " | Edited Table: " + message.EditedTable + " | Previous Version: " + message.PreviousVersion + " | New Version: " + message.NewVersion;

						// add to list 
						messages.Add(message);
					}
				}
				return messages;
			} catch (Exception ex) { throw new Exception(ex.Message); }
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

		public User.ActionTypes DeleteNotification(User u)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_USER_NOTIFICATIONS", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intUserNotificationID", u.Notification.NotificationID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return User.ActionTypes.DeleteSuccessful;
				} 
				else
				{
					return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes DeleteAdminNotifications(User u)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_ADMIN_NOTIFICATIONS", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intAdminNotificationID", u.AdminNotification.NotificationID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return User.ActionTypes.DeleteSuccessful;
				} else
				{
					return User.ActionTypes.Unknown;
				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Company.ActionTypes DeleteCompany(AdminVM vm)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_COMPANY", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", vm.Company.CompanyID, SqlDbType.BigInt);
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

		public Website.ActionTypes DeleteWebsite(Website w)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_WEBSITE", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intWebsiteID", w.intWebsiteID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1:
						return Website.ActionTypes.DeleteSuccessful;
					default:
						return Website.ActionTypes.Unknown;

				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public MemberRequest.ActionTypes DeleteMemberRequest(MemberRequest m)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_MEMBER_REQUEST", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intMemberID", m.MemberID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1:
						return MemberRequest.ActionTypes.DeleteSuccessful;
					default:
						return MemberRequest.ActionTypes.Unknown;

				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Image> GetLocationImages(long intLocationID = 0, long intLocationImageID = 0) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATION_IMAGES", cn);
				List<Image> imgs = new List<Image>();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (intLocationID > 0) SetParameter(ref da, "@intLocationID", intLocationID, SqlDbType.BigInt);
				if (intLocationImageID > 0) SetParameter(ref da, "@intLocationImageID", intLocationImageID, SqlDbType.BigInt);

				try {
					da.Fill(ds);
				}
				catch (Exception ex2) {
					//SysLog.UpdateLogFile(this.ToString(), MethodBase.GetCurrentMethod().Name.ToString(), ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					foreach (DataRow dr in ds.Tables[0].Rows) {
						Image i = new Image();
						i.ImageID = (long)dr["intLocationImageID"];
						i.ImageData = (byte[])dr["Image"];
						i.FileName = (string)dr["FileName"];
						i.Size = (long)dr["intImageSize"];
						imgs.Add(i);
					}
				}
				return imgs;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public long InsertLocationImage(Models.NewLocation loc) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_LOCATION_IMAGES", cn);

				SetParameter(ref cm, "@intLocationImageID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intLocationID", loc.lngLocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@image", loc.LocationImage.ImageData, SqlDbType.VarBinary);
				SetParameter(ref cm, "@file_name", loc.LocationImage.FileName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@image_size", loc.LocationImage.Size, SqlDbType.BigInt);

				cm.ExecuteReader();
				CloseDBConnection(ref cn);
				return (long)cm.Parameters["@intLocationImageID"].Value;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public SaleSpecial.ActionTypes DeleteSpecialLocation(SaleSpecial s, Location l)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_SPECIALLOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intSpecialID", s.SpecialID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return SaleSpecial.ActionTypes.DeleteSuccessful;
				}
				else
				{
					return SaleSpecial.ActionTypes.Unknown;
				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public CategoryItem.ActionTypes DeleteCategories(Location l, CategoryItem c)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("DELETE_CATEGORY_FROM_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intCategoryID", c.ItemID, SqlDbType.BigInt);
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

		public User.ActionTypes UpdateUser(ProfileViewModel vm)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_USER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intUserID", vm.User.UID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@strFirstName", vm.User.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strLastName", vm.User.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAddress", vm.User.Address, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", vm.User.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", vm.User.intState, SqlDbType.BigInt);
				SetParameter(ref cm, "@strZip", vm.User.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", vm.User.Phone, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", vm.User.Email, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPassword", vm.User.Password, SqlDbType.NVarChar);

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

		public User.ActionTypes UpdateUser_NotVM(Models.User user) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_USER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intUserID", user.UID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@strFirstName", user.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strLastName", user.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAddress", user.Address, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", user.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", user.intState, SqlDbType.BigInt);
				SetParameter(ref cm, "@strZip", user.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", user.Phone, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", user.Email, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPassword", user.Password, SqlDbType.NVarChar);

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

		public MemberRequest.ActionTypes UpdateMemberStatus(MemberRequest m)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_MEMBER_STATUS", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intMemberID", m.MemberID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					m.ActionType = MemberRequest.ActionTypes.InsertSuccessful;
					return m.ActionType;
				}

				m.ActionType = MemberRequest.ActionTypes.Unknown;
				return m.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Website.ActionTypes UpdateWebsite(Website w)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_WEBSITE", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intWebsiteID", w.intWebsiteID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strURL", w.strURL, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return Website.ActionTypes.UpdateSuccessful;
				}
				else
				{
					return Website.ActionTypes.Unknown;
				}

			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public SocialMedia.ActionTypes UpdateSocialMedia(SocialMedia s)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_SOCIALMEDIA", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanySocialMediaID", s.intCompanySocialMediaID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strSocialMediaLink", s.strSocialMediaLink, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return SocialMedia.ActionTypes.UpdateSuccessful;
				} else
				{
					return SocialMedia.ActionTypes.Unknown;
				}

			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Website.ActionTypes InsertNewWebsite(Website w, Company c)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_NEW_WEBSITE", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strURL", w.strURL, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intWebsiteTypeID", w.intWebsiteTypeID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return Website.ActionTypes.UpdateSuccessful;
				} else
				{
					return Website.ActionTypes.Unknown;
				}

			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public SocialMedia.ActionTypes InsertNewSocialMedia(SocialMedia s, Company c)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_NEW_SOCIALMEDIA", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@strSocialMediaLink", s.strSocialMediaLink, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intSocialMediaID", s.intSocialMediaID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return SocialMedia.ActionTypes.UpdateSuccessful;
				} else
				{
					return SocialMedia.ActionTypes.Unknown;
				}

			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public void UpdateCompanyInfo(Company c)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_COMPANY_INFO", cn);

				SetParameter(ref cm, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strCompanyName", c.Name, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAbout", c.About, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strBizYear", c.Year, SqlDbType.NVarChar);

				cm.ExecuteReader();

				CloseDBConnection(ref cn);
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Location.ActionTypes UpdateLocationInfo(Location l)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strAddress", l.Address, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strCity", l.City, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intStateID", l.intState, SqlDbType.BigInt);
				SetParameter(ref cm, "@strZip", l.Zip, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPhone", l.Phone, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", l.Email, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					return Location.ActionTypes.UpdateSuccessful;
				}
				else
				{
					return Location.ActionTypes.Unknown;
				}

			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes UpdateNotificationStatus(User u)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("MARK_NOTIFICATION_AS_READ", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intUserNotificationID", u.Notification.NotificationID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					u.ActionType = User.ActionTypes.UpdateSuccessful;
				}
				else
				{
					u.ActionType = User.ActionTypes.Unknown;
				}

				return u.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes UpdateAdminNotificationStatus(User u)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("MARK_ADMIN_NOTIFICATION_AS_READ", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intAdminNotificationID", u.AdminNotification.NotificationID, SqlDbType.SmallInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					u.ActionType = User.ActionTypes.UpdateSuccessful;
				} else
				{
					u.ActionType = User.ActionTypes.Unknown;
				}

				return u.ActionType;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Company.ActionTypes InsertNewCompany(AdminVM vm)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_COMPANY", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCompanyID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strCompanyName", vm.Company.Name, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strAbout", vm.Company.About, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strBizYear", vm.Company.Year, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case -1:
						return Company.ActionTypes.DuplicateName;
					case 1: // new user created
						vm.Company.CompanyID = Convert.ToInt16(cm.Parameters["@intCompanyID"].Value);
						return Company.ActionTypes.InsertSuccessful;
					default:
						return Company.ActionTypes.Unknown;
				}

			}
			catch (Exception ex) { throw new Exception(ex.Message); }

		}

		public List<Website> GetCompanyWebsites(Company c)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_COMPANY_WEBSITES", cn);

				SetParameter(ref da, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);

				// create list object that will hold list of websites 
				List<Website> websites = new List<Website>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new website object for each website
						Website w = new Website();

						// add data to object
						w.intWebsiteID = Convert.ToInt16(dr["intWebsiteID"]);
						w.intWebsiteTypeID = Convert.ToInt16(dr["intWebsiteTypeID"]);
						w.strWebsiteType = (string)dr["strWebsiteType"];
						w.strURL = (string)dr["strURL"];

						// add object to list of websites 
						websites.Add(w);
					}
				}
				return websites;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<SocialMedia> GetCompanySocialMedia(Company c)
		{
			try
			{
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("GET_COMPANY_SOCIALMEDIA", cn);

				SetParameter(ref da, "@intCompanyID", c.CompanyID, SqlDbType.BigInt);

				// create list object that will hold list of websites 
				List<SocialMedia> links = new List<SocialMedia>();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try { da.Fill(ds); } catch (Exception ex) { throw new Exception(ex.Message); } 
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						// create new website object for each website
						SocialMedia s = new SocialMedia();

						// add data to object
						s.intSocialMediaID = Convert.ToInt16(dr["intSocialMediaID"]);
						s.strPlatform = (string)dr["strPlatform"];
						s.intCompanySocialMediaID = Convert.ToInt16(dr["intCompanySocialMediaID"]);
						s.strSocialMediaLink = (string)dr["strSocialMediaLink"];

						// add object to list of websites 
						links.Add(s);
					}
				}
				return links;
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public void SendUserNotification(MemberRequest m, int intNotificationID, int intNotificationStatusID)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_USER_NOTIFICATION", cn);

				SetParameter(ref cm, "@intUserID", m.UserID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intNotificationID", intNotificationID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intNotificationStatusID", intNotificationStatusID, SqlDbType.SmallInt);

				cm.ExecuteReader();

				CloseDBConnection(ref cn);
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public SaleSpecial InsertSpecial(SaleSpecial s) {
			try 
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_SPECIAL", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intSpecialID", null, SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strDescription", s.strDescription, SqlDbType.NVarChar);
				SetParameter(ref cm, "@monPrice", s.monPrice, SqlDbType.Money);
				SetParameter(ref cm, "@dtmStart", s.dtmStart, SqlDbType.Date);
				SetParameter(ref cm, "@dtmEnd", s.dtmEnd, SqlDbType.Date);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) {
					s.SpecialID = Convert.ToInt16(cm.Parameters["@intSpecialID"].Value);
					return s;
				}
				return s;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public void InsertAdminNotification(User u, int editedColumnID, int editedTableID, string previousVersion, string newVersion)
		{
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ADMIN_NOTIFICATION_EDIT", cn);

				SetParameter(ref cm, "@intUserID", u.UID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intEditedColumnID", editedColumnID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intEditedTableID", editedTableID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@strPreviousVersion", previousVersion, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strNewVersion", newVersion, SqlDbType.NVarChar);

				cm.ExecuteReader();

				CloseDBConnection(ref cn);
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public SaleSpecial.ActionTypes InsertSpecialLocation(SaleSpecial s, Location l) 
		{
			try 
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_SPECIALLOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intSpecialLocationID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intSpecialID", s.SpecialID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1) {
					return SaleSpecial.ActionTypes.InsertSuccessful;
				}
				return SaleSpecial.ActionTypes.Unknown;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public CategoryItem.ActionTypes InsertCategories(CategoryItem c, Location l)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_CATEGORYLOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intCategoryLocationID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intCategoryID", c.ItemID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intLocationID", l.LocationID, SqlDbType.BigInt);
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

					//Reset Company ID for Display table storage
					locList.lstLocations[i].lngCompanyID = 0;

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

		public NewLocation.ActionTypes AddNewLocation(AdminVM vm)
        {
			try
            {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_LOCATION", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intLocationID", null, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intCompanyID", vm.Company.CompanyID, SqlDbType.BigInt);
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

						/*
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
						*/

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
						//if (item.strContactFirstName == string.Empty || item.strContactLastName == string.Empty) continue;
						//if (item.contactPhone.AreaCode == string.Empty || item.contactPhone.Prefix == string.Empty) continue;

						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_CONTACTLOCATION_RELATIONSHIP", cn);
						int intReturnValue = -1;

						SetParameter(ref cm, "@intContactPersonID", item.lngContactPersonID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
						SetParameter(ref cm, "@intContactLocationID", item.lngContactLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
						SetParameter(ref cm, "@strContactName", item.strFullName, SqlDbType.NVarChar);
						SetParameter(ref cm, "@strContactPhone", item.strFullPhone, SqlDbType.NVarChar);
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

		public List<Models.CompanyMember> GetCompanyMembers() {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_MEMBER_COMPANIES", cn);
				List<Models.CompanyMember> lstCompanyMember = new List<Models.CompanyMember>();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				try {
					da.Fill(ds);
				}
				catch (Exception ex2) {
					throw new Exception(ex2.Message);
				}
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					foreach (DataRow dr in ds.Tables[0].Rows) {
						CompanyMember companyMember = new CompanyMember();
						companyMember.lngCompanyID = (long)dr["intCompanyID"];
						companyMember.lngMemberID = (short)dr["intMemberID"];
						companyMember.lngCompanyMemberID = (short)dr["intCompanyMemberID"];
						lstCompanyMember.Add(companyMember);
					}
				}
				return lstCompanyMember;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.Days> GetLandingHours(long lngLocationID = 0) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_LOCATIONHOURS", cn);
				List<Models.Days> lstDays = new List<Models.Days>();

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
						Models.Days day = new Models.Days();
						day.strDay = (string)dr["strDay"];
						day.strOpenTime = (string)dr["strOpen"];
						day.strClosedTime = (string)dr["strClose"];
						lstDays.Add(day);
					}
				}
				return lstDays;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Models.Days> GetTempLocationHours(long lngLocationID = 0) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_TEMP_LOCATIONHOURS", cn);
				List<Models.Days> lstDays = new List<Models.Days>();

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
						Models.Days day = new Models.Days();
						day.strDay = (string)dr["strDay"];
						day.strOpenTime = (string)dr["strOpen"];
						day.strClosedTime = (string)dr["strClose"];
						day.intDayID = (short)dr["intDayID"];
						lstDays.Add(day);
					}
				}
				return lstDays;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Models.NewLocation GetTempLocation(short AdminRequestKey) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_TEMP_LOCATION", cn);
				Models.NewLocation loc = new Models.NewLocation();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				if (AdminRequestKey > 0) SetParameter(ref da, "@intAdminRequestKey", AdminRequestKey, SqlDbType.BigInt);
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
						loc.CompanyName = (string)dr["strCompanyName"];
						loc.StreetAddress = (string)dr["strAddress"];
						loc.City = (string)dr["strCity"];
						loc.State = (string)dr["strState"];
						loc.intState = (short)dr["intStateID"];
						loc.Zip = (string)dr["strZip"];
						loc.strFullPhone = (string)dr["strPhone"];
						loc.BusinessEmail = (string)dr["strEmail"];
						loc.Bio = (string)dr["strAbout"];
						loc.BizYear = (string)dr["strBizYear"];
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

		public List<Models.CategoryItem> GetTempCategories(long lngLocationID = 0) {
			List<Models.CategoryItem> lstCategories = new List<CategoryItem>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_ALLCATEGORY_FORTEMPLOCATION", cn);

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
						item.SpecialID = Convert.ToInt16(dr["intSpecialID"]);
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

		public List<Models.ContactPerson> GetTempContacts(long lngLocationID = 0) {
			List<Models.ContactPerson> lstContactPerson = new List<ContactPerson>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_TEMP_LOCATION_CONTACTS", cn);

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
						item.strFullPhone = (string)dr["strContactPhone"];
						item.strContactEmail = (string)dr["strContactEmail"];
						item.intContactTypeID = (short)dr["intContactPersonTypeID"];
						item.strContactPersonType = (string)dr["strContactPersonType"];
						item.intLocationID = (long)dr["intLocationID"];
						item.intCompanyID = (long)dr["intCompanyID"];
						item.intContactPersonID = (long)dr["intContactPersonID"];
						lstContactPerson.Add(item);
					}
				}
				return lstContactPerson;
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
						item.strFullPhone = (string)dr["strContactPhone"];
						item.strContactEmail = (string)dr["strContactEmail"];
						item.intContactTypeID = (short)dr["intContactPersonTypeID"];
						item.strContactPersonType = (string)dr["strContactPersonType"];
						item.intLocationID = (long)dr["intLocationID"];
						item.intCompanyID = (long)dr["intCompanyID"];
						item.intContactPersonID = (long)dr["intContactPersonID"];
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

		public List<Models.SocialMedia> GetTempSocialMedia(long lngLocationID = 0) {
			List<Models.SocialMedia> lstSocialMedia = new List<SocialMedia>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_TEMP_LOCATION_SOCIALMEDIA", cn);

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
						item.intSocialMediaID = (short)dr["intSocialMediaID"];
						item.intCompanyID = (long)dr["intCompanyID"];
						item.blnAvailable = true;
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

		public List<Models.Website> GetTempWebsite(long lngLocationID = 0) {
			List<Models.Website> lstWebites = new List<Website>();
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("SELECT_TEMP_LOCATION_WEBSITE", cn);

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

		public bool InsertUserToMember(User u)
		{

			try
			{
				SqlConnection cn = null; // inside System.Data.SqlClient
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_USER_TO_MEMBER", cn);
				int intReturnValue = -1;

				// set values of member and payment type to string to create a switch 
				string mt = u.MemberShipType;
				string pt = u.PaymentType;
				int imt = 0;
				int ipt = 0;


				switch (mt)
				{					
					case "Associate":
						imt = 1;
						break;

					case "Business":
						imt = 2;
						break;

					case "Allied":
						imt = 3;
						break;

					default:
						imt = 0;
						break;
				}

				switch (pt)
				{
					case "Zelle":
						ipt = 1;
						break;

					case "Check":
						ipt = 2;
						break;
					
					default:
						ipt = 0;
						break;
				}

				SetParameter(ref cm, "@intNewMemberID", "null", SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@intUserID", u.UID, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intMemberLevelID", imt, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intPaymentTypeID", ipt, SqlDbType.NVarChar);
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				if (intReturnValue == 1)
				{
					//vm.MainBanner.BannerID = Convert.ToInt16(cm.Parameters["@intNewBannerID"].Value);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public LocationList.ActionTypes InsertTempCompany(LocationList locList) {
			int i = 0;
			do {
				try {
					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_TEMP_COMPANY", cn);
					int intReturnValue = -1;

					if(string.IsNullOrEmpty(locList.lstLocations[i].Bio)) {
						locList.lstLocations[i].Bio = string.Empty;
					}

					if(string.IsNullOrEmpty(locList.lstLocations[i].BizYear)) {
						locList.lstLocations[i].BizYear = string.Empty;
					}

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

		public LocationList.ActionTypes InsertAdminRequest(Models.AdminRequest adminRequest ) {
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ADMIN_REQUEST", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@intAdminRequestID", adminRequest.intAdminRequest, SqlDbType.SmallInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strRequestType", adminRequest.strRequestType, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strRequestedChange", adminRequest.strRequestedChange, SqlDbType.NVarChar);
				SetParameter(ref cm, "@intApprovalStatusID", adminRequest.intApprovalStatusID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intMemberID", adminRequest.intMemberID, SqlDbType.SmallInt);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();


				CloseDBConnection(ref cn);
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				if (intReturnValue == -1) return Models.LocationList.ActionTypes.CompanyNameExists;
				adminRequest.intAdminRequest = (short)cm.Parameters["@intAdminRequestID"].Value;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }

			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertTempLocations(LocationList locList) {
			int i = 0;
			do {
				try {
					//Convert Phone Class to Concat String
					string PhoneNumber = locList.lstLocations[i].BusinessPhone.AreaCode + locList.lstLocations[i].BusinessPhone.Prefix + locList.lstLocations[i].BusinessPhone.Suffix;

					SqlConnection cn = null;
					if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
					SqlCommand cm = new SqlCommand("INSERT_TEMP_LOCATION", cn);
					int intReturnValue = -1;

					SetParameter(ref cm, "@intLocationID", locList.lstLocations[i].lngLocationID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
					SetParameter(ref cm, "@intCompanyID", locList.lstLocations[i].lngCompanyID, SqlDbType.BigInt);
					SetParameter(ref cm, "@strAddress", locList.lstLocations[i].LocationName, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strCity", locList.lstLocations[i].City, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intStateID", locList.lstLocations[i].intState, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strZip", locList.lstLocations[i].Zip, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strPhone", PhoneNumber, SqlDbType.NVarChar);
					SetParameter(ref cm, "@strEmail", locList.lstLocations[i].BusinessEmail, SqlDbType.NVarChar);
					SetParameter(ref cm, "@intAdminRequestID", locList.adminReq.intAdminRequest, SqlDbType.SmallInt);
					

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

		public LocationList.ActionTypes InsertTempSpecialties(LocationList locList, List<Models.CategoryItem>[] categories) {
			int intReturnValue = 0;
			int i = 0;
			do {
				try {
					foreach (Models.CategoryItem item in categories[i]) {
						if (item.blnAvailable == true) {
							SqlConnection cn = null;
							if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
							SqlCommand cm = new SqlCommand("INSERT_TEMP_CATEGORYLOCATION", cn);

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

		public LocationList.ActionTypes InsertTempLocationHours(LocationList locList, List<Models.Days>[] LocationHours) {
			int i = 0;
			int intReturnValue = -1;
			try {
				do {
					foreach (Models.Days item in LocationHours[i]) {

						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_TEMP_LOCATIONHOURS", cn);

						
						
						if (!String.IsNullOrEmpty(item.strOpenTime)) {
							item.dtOpenTime = Convert.ToDateTime(item.strOpenTime);
							item.strOpenTime = item.dtOpenTime.ToShortTimeString();
						}
						else item.strOpenTime = "Closed";

						if (!String.IsNullOrEmpty(item.strClosedTime)) {
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

		public LocationList.ActionTypes InsertTempSocialMedia(LocationList locList, List<Models.SocialMedia>[] socialMedias) {
			int intReturnValue = 0;
			int i = 0;
			do {
				try {

					foreach (Models.SocialMedia item in socialMedias[i]) {
						if (item.blnAvailable == false) continue;
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_TEMP_SOCIALMEDIA", cn);

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

		public LocationList.ActionTypes InsertTempContactPerson(LocationList locList, List<Models.ContactPerson>[] contacts) {
			int i = 0;
			do {
				try {
					foreach (Models.ContactPerson item in contacts[i]) {
						if (!string.IsNullOrEmpty(item.strContactFirstName) && !string.IsNullOrEmpty(item.strContactLastName)) {
							string name = item.strContactLastName + ", " + item.strContactFirstName;
							string phone = "(" + item.contactPhone.AreaCode + ") " + item.contactPhone.Prefix + "-" + item.contactPhone.Suffix;


							//if (item.strContactFirstName == string.Empty || item.strContactLastName == string.Empty) continue;
							//if (item.contactPhone.AreaCode == string.Empty || item.contactPhone.Prefix == string.Empty) continue;

							SqlConnection cn = null;
							if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
							SqlCommand cm = new SqlCommand("INSERT_TEMP_CONTACTLOCATION_RELATIONSHIP", cn);
							int intReturnValue = -1;

							SetParameter(ref cm, "@intContactPersonID", item.lngContactPersonID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
							SetParameter(ref cm, "@intContactLocationID", item.lngContactPersonID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
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

		public LocationList.ActionTypes InsertTempWebsite(LocationList locList, List<Models.Website>[] websites) {
			int i = 0;
			int intReturnValue = 0;
			do {
				try {
					foreach (Models.Website item in websites[i]) {
						if (item.strURL == string.Empty || item.strURL == null) continue;
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_TEMP_WEBSITE", cn);

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

		public List<Models.AdminRequest> GetLocationRequests() {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("SELECT_ADMINREQUESTS", cn);

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				List<Models.AdminRequest> lstAdminRequests = new List<Models.AdminRequest>();

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows) {
						// create new Company object
						AdminRequest adminRequest = new AdminRequest();

						// add values to CompanyID and Name properties 
						adminRequest.intAdminRequest = (short)(dr["intAdminRequestID"]);
						adminRequest.intMemberID = (short)dr["intMemberID"];
						adminRequest.strRequestType = (string)dr["strRequestType"];
						adminRequest.strRequestedChange = (string)dr["strRequestedChange"];
						adminRequest.intApprovalStatusID = (short)dr["intApprovalStatusID"];

						// add Company object (c) to Company list (companies) 
						lstAdminRequests.Add(adminRequest);
					}
				}
				// return list of companies 
				return lstAdminRequests;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public Models.AdminRequest GetSingleLocationRequest(short intAdminRequestID) {
			try {
				DataSet ds = new DataSet();
				SqlConnection cn = new SqlConnection();

				// try to connect to database -- throw error if unsuccessful
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect.");

				// specify which stored procedure we are using 
				SqlDataAdapter da = new SqlDataAdapter("SELECT_SINGLE_ADMINREQUEST", cn);

				AdminRequest adminRequest = new AdminRequest();

				// set command type as stored procedure
				da.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (intAdminRequestID > 0) SetParameter(ref da, "@intAdminRequestID", intAdminRequestID, SqlDbType.SmallInt);

				try { da.Fill(ds); }
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally { CloseDBConnection(ref cn); }

				if (ds.Tables[0].Rows.Count != 0) {
					// loop through results and add to list 
					foreach (DataRow dr in ds.Tables[0].Rows) {
						// add values to CompanyID and Name properties 
						adminRequest.intAdminRequest = (short)(dr["intAdminRequestID"]);
						adminRequest.intMemberID = (short)dr["intMemberID"];
						adminRequest.strRequestType = (string)dr["strRequestType"];
						adminRequest.strRequestedChange = (string)dr["strRequestedChange"];
						adminRequest.intApprovalStatusID = (short)dr["intApprovalStatusID"];

					}
				}
				// return list of companies 
				return adminRequest;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public LocationList.ActionTypes InsertCompanyMember(long intCompanyID, short intMemberID) {
			int intReturnValue = 0;
				try {
						SqlConnection cn = null;
						if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
						SqlCommand cm = new SqlCommand("INSERT_COMPANYMEMBER_RELATIONSHIP", cn);

						SetParameter(ref cm, "@intMemberID", intMemberID, SqlDbType.SmallInt);
						SetParameter(ref cm, "@intCompanyID", intCompanyID, SqlDbType.BigInt);

						SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

						cm.ExecuteReader();

						CloseDBConnection(ref cn);
						intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
						if (intReturnValue != 1) return Models.LocationList.ActionTypes.Unknown;
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
			return LocationList.ActionTypes.InsertSuccessful;
		}

		public LocationList.ActionTypes InsertMemberRequest(User u) {
			int intReturnValue = 0;
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_MEMBER_REQUEST", cn);
				short approvalStatus = 1;
				short paymentStatus = 1;

				SetParameter(ref cm, "@intUserID", u.UID, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intMemberLevel", u.intMembershipType, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intPaymentType", u.intPaymentType, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intApprovalStatus", approvalStatus, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intPaymentStatus", paymentStatus, SqlDbType.SmallInt);
				SetParameter(ref cm, "@intMemberID", u.intMemberID, SqlDbType.SmallInt, Direction: ParameterDirection.Output);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				CloseDBConnection(ref cn);
				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				if (intReturnValue != 1) return Models.LocationList.ActionTypes.Unknown;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
			return LocationList.ActionTypes.InsertSuccessful;
		}

		
	}
}
