using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace GCRBA.Models
{
	public class Database
	{
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
						newUser.intStateID = Convert.ToInt16(dr["intStateID"]);
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
	}
}