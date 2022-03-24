using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace GCRBA.Models
{
	public class Database
	{
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
				SetParameter(ref cm, "@user_id", u.UserName, SqlDbType.NVarChar);
				
				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				// once this line completes, it will return the return value from the db (if 1 then good)
				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1: // new user created
						u.UID = (long)cm.Parameters["@uid"].Value;
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

		public User Login(User u)
		{
			try
			{
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("LOGIN", cn);
				DataSet ds;
				User newUser = null;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@user_id", u.UserName, SqlDbType.NVarChar);
				SetParameter(ref da, "@password", u.Password, SqlDbType.NVarChar);

				try
				{
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0)
					{
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.UID = (long)dr["UID"];
						newUser.UserName = u.UserName;
						newUser.Password = u.Password;
						newUser.FirstName = (string)dr["FirstName"];
						newUser.LastName = (string)dr["LastName"];
						newUser.Email = (string)dr["Email"];
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
				{
					CloseDBConnection(ref cn);
				}
				return newUser; //alls well in the world
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
				SetParameter(ref cm, "@user_name", u.UserName, SqlDbType.NVarChar);
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

		private bool GetDBConnection(ref SqlConnection SQLConn)
		{
			try
			{
				if (SQLConn == null) SQLConn = new SqlConnection();
				if (SQLConn.State != ConnectionState.Open)
				{
					SQLConn.ConnectionString = ConfigurationManager.AppSettings["AppDBConnect"];
					SQLConn.Open();
				}
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		private bool CloseDBConnection(ref SqlConnection SQLConn)
		{
			try
			{
				if (SQLConn.State != ConnectionState.Closed)
				{
					SQLConn.Close();
					SQLConn.Dispose();
					SQLConn = null;
				}
				return true;
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