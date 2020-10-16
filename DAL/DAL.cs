using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Data.OleDb;

namespace DataAccess
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SQLServer
	{
		public SQLServer(string ConnectionString)
		{
			//            MyBase.New()
			privateConnectionString = ConnectionString;
			privateModuleName = this.GetType().ToString();
		}

		public SQLServer(string Server, string Database, string UserName, string Password)
		{
			//            MyBase.New()
			privateConnectionString = "Server=" + Server + ";Database=" + Database + ";User ID=" + UserName + ";Password=" + Password + ";";
			privateModuleName = this.GetType().ToString();
		}

		private SqlConnection privateConnection ;
		private SqlCommand privateCommand;
		private SqlDataReader privateDataReader;
		private XmlReader privateXMLReader;
		private SqlDataAdapter privateSQLDataAdapter;
		private DataSet privateDataSet;

		private ArrayList privateParameterList = new ArrayList();

		private String privateConnectionString;
		private String privateDatabase;
		private String privateUserName;
		private String privatePassword;

		private String privateModuleName;
		private Boolean privateDisposedBoolean = false;
		private const String privateExceptionMessage = "Data Application Error. Detail Error Information can be found in the Application Log";


		//Region "private Variables and Objects"
		private string ConnectionString
		{
			get
			{
				try
				{
					return privateConnection.ConnectionString;
				}
				catch(Exception e)
				{
					return e.Message;
				}
			}
			set
			{
				privateConnectionString = value;
			}
		}

		private string Server
		{
			get
			{
				try
				{
					return privateUserName;
				}
				catch(Exception e)
				{
					return e.Message;
				}
			}
			set
			{
				privateUserName = value;
			}
		}

		private string Database
		{
			get
			{
				try
				{
					return privateDatabase;
				}
				catch(Exception e)
				{
					return e.Message;
				}
			}
			set
			{
				privateDatabase = value;
			}
		}

		private string UserName
		{
			get
			{
				try
				{
					return privateUserName;
				}
				catch(Exception e)
				{
					return e.Message;
				}
			}
			set
			{
				privateUserName = value;
			}
		}

		private string Password
		{
			get
			{
				try
				{
					return privatePassword;
				}
				catch(Exception e)
				{
					return e.Message;
				}
			}
			set
			{
				privatePassword = value;
			}
		}

		public enum SQLDataType {SQLString,SQLChar,	SQLInteger,	SQLBit,SQLDateTime,SQLDecimal,SQLMoney,SQLImage, SQLNVarchar};


		public  DataSet runSQLDataSet(string SQL,String TableName )
		{
			//Validate the SQL String to be larger than 10 characters
			ValidateSQLStatement(SQL);
			//We include all called object in the try block to catch any excepitons that could occur ( even in the creation of the connection)
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean )
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it.");
				//Set a new Connecton
				privateConnection = new SqlConnection(privateConnectionString);
				//Set a new Command that accepts an SQL statement and the connection. 
				//The command.commandtype does not have to be set since it defaults to text
				privateCommand = new SqlCommand(SQL, privateConnection);
				privateCommand.CommandTimeout = 3600;
				//Set a new DataSet
				privateDataSet = new DataSet();
				//Set a new DataAdapter that will run the SQL statement
				privateSQLDataAdapter = new SqlDataAdapter(privateCommand);
				//Depending on table name passed, Fill the new DataSet with the returned Data in a table
				if (TableName == null) 
					privateSQLDataAdapter.Fill(privateDataSet);
				else
					privateSQLDataAdapter.Fill(privateDataSet, TableName);

				return privateDataSet;
			}
			catch (Exception ExceptionObject)
			{
				//Any exception will be logged through our private logexception function
				LogException(ExceptionObject);
				//The exception is passed back to the calling code, with our custom message and specific exception information
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
			finally
			{
				//Immediately Close the Connection after use to free up resources
				privateConnection.Close();
			}
		}

		public SqlDataReader runSQLDataReader(String SQL)
		{
			//The runSQLDataReader function accepts a SQL statement that is required
			//Validate the SQL String to be larger than 10 characters
			ValidateSQLStatement(SQL);
			//We include all called object in the try block to catch any excepitons that could occur ( even in the creation of the connection)
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean )
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it.");

				//Set a new Connecton
				privateConnection = new SqlConnection(privateConnectionString);
				//Set a new Command that accepts an SQL statement and the connection. 
				//The command.commandtype does not have to be set since it defaults to text
				privateCommand = new SqlCommand(SQL, privateConnection);
				//We need to open the connection for the DataReader explicitly
				privateConnection.Open();
				//Run the Execute Reader method of the Command Object
				privateDataReader = privateCommand.ExecuteReader();
				return privateDataReader;
			}
			catch (Exception ExceptionObject)
			{
				//Any exception will be logged through our private logexception function
				LogException(ExceptionObject);
				//If an exception occurs, close the connection now!
				privateConnection.Close();
				//The exception is passed back to the calling code, with our custom message and specific exception information
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
		}

		public void runSQLXML(String SQL)
		{
			//Validate the SQL String to be larger than 10 characters
			ValidateSQLStatement(SQL);
			//We include all called object in the try block to catch any excepitons that could occur ( even in the creation of the connection)
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean )
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it.");

				//Set a new Connecton
				privateConnection = new SqlConnection(privateConnectionString);
				//Set a new Command that accepts an SQL statement and the connection. 
				//The command.commandtype does not have to be set since it defaults to text
				privateCommand = new SqlCommand(SQL, privateConnection);
				//We need to open the connection for the DataReader explicitly
				privateConnection.Open();
				//Run the Execute NonQuery method of the Command Object
				privateCommand.ExecuteNonQuery();
			}
			catch (Exception ExceptionObject ) 
			{
				//Any exception will be logged through our private logexception function
				LogException(ExceptionObject);
				//The exception is passed back to the calling code, with our custom message and specific exception information
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
			finally
			{
				//Immediately Close the XMLReader andConnection after use to free up resources
				privateXMLReader.Close();
				privateConnection.Close();
			}
		}

		public DataSet runSPDataSet(String SPName,String TableName)
		{
			//Validate Stored Procedure
			ValidateSPStatement(SPName);
			//Setting the objects to handle parameters
			Parameter privateUsedParameter;//will return the specific parameter in the privateParameterList
			SqlParameter privateParameter;//will contain the converted SQLParameter
			//The usedEnumerator makes it easy to step through the list of parameters in the privateParameterList
			IEnumerator usedEnumerator = privateParameterList.GetEnumerator();
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean )
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it");
				//Set a new connection and DataSet
				privateConnection = new SqlConnection(privateConnectionString);
				DataSet privateDataSet = new DataSet();

				//Define the command object and set commandtype to process Stored Procedure
				privateCommand = new SqlCommand(SPName, privateConnection);
				privateCommand.CommandType = CommandType.StoredProcedure;
				privateCommand.CommandTimeout = 12000;

				//Move through the privateParameterList wiht the help of the enumerator
				while (usedEnumerator.MoveNext())
				{
					privateUsedParameter = null;
					//Get parameter in privateParameterList
					privateUsedParameter = (Parameter) usedEnumerator.Current;
					//Convert paramter to SQLParameter
					privateParameter = ConvertParameters(privateUsedParameter);
					//Add converted parameter to the privateCommand object that imports data through the DataAdapter
					privateCommand.Parameters.Add(privateParameter);
				}
				//Have the DataAdapter run the Stored Procedure
				privateSQLDataAdapter = new SqlDataAdapter(privateCommand);
				//Depending on table name passed, create the DataSet with or without specifically naming the table
				if (TableName == null)
					privateSQLDataAdapter.Fill(privateDataSet);
				else
					privateSQLDataAdapter.Fill(privateDataSet, TableName);

				return privateDataSet;
			}
			catch (Exception ExceptionObject)
			{
				//An exception will be logged thorugh our private logexception funciton
				LogException(ExceptionObject);
				//The exception is passed to the calling code
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
			finally
			{
				//Always close the connection as soon as possible(only then will object be allowed to go out of scope)
				privateConnection.Close();
			}
		}

		public SqlDataReader runSPDataReader(string SPName)
		{
			//Validate Stored Procedure
			ValidateSPStatement(SPName);
			//Setting the objects to handle parameters
			Parameter privateUsedParameter;            //will return the specific parameter in the privateParameterList
			SqlParameter privateParameter;//will contain the converted SQLParameter
			//The usedEnumerator makes it easy to step through the list of parameters in the privateParameterList
			IEnumerator usedEnumerator = privateParameterList.GetEnumerator();
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean)
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it");

				//Set a new connection and DataSet
				privateConnection = new SqlConnection(privateConnectionString);
				//Define the command object and set commandtype to process Stored Procedure
				privateCommand = new SqlCommand(SPName, privateConnection);
				privateCommand.CommandType = CommandType.StoredProcedure;
				//Move through the privateParameterList wiht the help of the enumerator
				while (usedEnumerator.MoveNext())
				{
					privateUsedParameter = null;
					//Get parameter in privateParameterList
					privateUsedParameter = (Parameter) usedEnumerator.Current;
					//Convert paramter to SQLParameter
					privateParameter = ConvertParameters(privateUsedParameter);
					//Add converted parameter to the privateCommand object that imports data through the DataAdapter
					privateCommand.Parameters.Add(privateParameter);
				}

				privateConnection.Open();
				privateDataReader = privateCommand.ExecuteReader();
				return privateDataReader;
			}
			catch (Exception ExceptionObject)
			{
				//An exception will be logged thorugh our private logexception funciton
				LogException(ExceptionObject);
				//The exception is passed back to the calling code, with our custom message and specific exception information
				privateConnection.Close();
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
		}

		public String runSPXMLReader(String SPName)
		{
			//Validate Stored Procedure
			ValidateSPStatement(SPName);
			//Setting the objects to handle parameters
			Parameter privateUsedParameter;//will return the specific parameter in the privateParameterList
			SqlParameter privateParameter;//will contain the converted SQLParameter
			//The usedEnumerator makes it easy to step through the list of parameters in the privateParameterList
			IEnumerator usedEnumerator = privateParameterList.GetEnumerator();
			String privateXMLString = "";                  //We need this string to build the XML Statement

			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean)
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it");

				//Set a new connection and DataSet
				privateConnection = new SqlConnection(privateConnectionString);

				//Define the command object and set commandtype to process Stored Procedure
				privateCommand = new SqlCommand(SPName, privateConnection);
				privateCommand.CommandType = CommandType.StoredProcedure;

				//Move through the privateParameterList wiht the help of the enumerator
				while (usedEnumerator.MoveNext())
				{
					privateUsedParameter = null;
					//Get parameter in privateParameterList
					privateUsedParameter = (Parameter)usedEnumerator.Current;
					//Convert paramter to SQLParameter
					privateParameter = ConvertParameters(privateUsedParameter);
					//Add converted parameter to the privateCommand object that imports data through the DataAdapter
					privateCommand.Parameters.Add(privateParameter);
				}

				privateConnection.Open();
				privateXMLReader = privateCommand.ExecuteXmlReader();
				//Build the XML string
				while(privateXMLReader.Read() == true)
				{
					privateXMLString += privateXMLReader.ReadOuterXml() + "<BR>";
				}
				

				return privateXMLString;
			}
			catch (Exception ExceptionObject)
			{
				//An exception will be logged thorugh our private logexception funciton
				LogException(ExceptionObject);
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
			finally
			{
				//Close the XMLReader and the Connection - we don't need it anymore
				privateXMLReader.Close();
				privateConnection.Close();
			}

		}			

		public ArrayList runSPOutput(String SPName)
		{
			//Validate Stored Procedure
			ValidateSPStatement(SPName);

			//Setting the objects to handle parameters
			Parameter privateUsedParameter ;//will return the specific parameter in the privateParameterList
			SqlParameter privateParameter; //will contain the converted SQLParameter
			//The usedEnumerator makes it easy to step through the list of parameters in the privateParameterList
			IEnumerator usedEnumerator = privateParameterList.GetEnumerator();
			ArrayList outputParameters = new ArrayList();            //We need this arraylist to hold output parameters
//			SqlParameter privateParameterOut = new SqlParameter();         //Helps to create the output parameter array

			//Try
			//Check to see if this object has already been disposed
			if (privateDisposedBoolean)
				throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it");

			//Set a new connection and DataSet
			privateConnection = new SqlConnection(privateConnectionString);
			//Define the command object and set commandtype to process Stored Procedure
			privateCommand = new SqlCommand(SPName, privateConnection);
			privateCommand.CommandType = CommandType.StoredProcedure;

			//Move through the privateParameterList with the help of the enumerator
			while (usedEnumerator.MoveNext())
			{
				privateUsedParameter = null;
				//Get parameter in privateParameterList
				privateUsedParameter = (Parameter) usedEnumerator.Current;
				//Convert paramter to SQLParameter
				privateParameter = ConvertParameters(privateUsedParameter);
				//Add converted parameter to the privateCommand object that imports data through the DataAdapter
				privateCommand.Parameters.Add(privateParameter);
			}

			privateConnection.Open();
			privateCommand.ExecuteNonQuery();

			//Iterate through all output parameters and return values
			foreach (SqlParameter privateParameterOut in privateCommand.Parameters)
				if (privateParameterOut.Direction == ParameterDirection.Output || privateParameterOut.Direction == ParameterDirection.ReturnValue )
					//Add each output and return value to our output paramterlist
					outputParameters.Add(privateParameterOut.Value);



			//Return the array list of output parameter values
			return outputParameters;

			//'Catch ExceptionObject As Exception
			//'An exception will be logged thorugh our private logexception funciton
			//'   LogException(ExceptionObject)
			//'  Throw New Exception(privateExceptionMessage, ExceptionObject)
			//'Finally
			//'Cose the Connection
            //privateConnection.Close();
			//'End Try
		}	

		public DataSet runSQLACCESSDataSet(String SQL,String TableName)
		{
			ValidateSQLStatement(SQL);

			//We declare our connection, command and DataAdapter variables here
			OleDbConnection privateOLEDBConnection = null;
			OleDbCommand privateOLEDBCommand;
			OleDbDataAdapter privateOLEDBDataAdapter;

			//We include all called object in the try block to catch any excepitons that could occur ( even in the creation of the connection)
			try
			{
				//Check to see if this object has already been disposed
				if (privateDisposedBoolean)
					throw new ObjectDisposedException(privateModuleName, "This object has already been disposed. You cannot reuse it.");

				//Set a new Connecton
				privateOLEDBConnection = new OleDbConnection(privateConnectionString);
				//Set a new Command that accepts an SQL statement and the connection. 
				//The command.commandtype does not have to be set since it defaults to text
				privateOLEDBCommand = new OleDbCommand(SQL, privateOLEDBConnection);
				//Set a new DataSet
				privateDataSet = new DataSet();
				//Set a new DataAdapter that will run the SQL statement
				privateOLEDBDataAdapter = new OleDbDataAdapter(privateOLEDBCommand);
					//Depending on table name passed, Fill the new DataSet with the returned Data in a table
					if (TableName == null)
						privateOLEDBDataAdapter.Fill(privateDataSet);
					else
						privateOLEDBDataAdapter.Fill(privateDataSet, TableName);

				return privateDataSet;
			}
			catch (Exception ExceptionObject)
			{
				//Any exception will be logged through our private logexception function
				LogException(ExceptionObject);
				//The exception is passed back to the calling code, with our custom message and specific exception information
				throw new Exception(privateExceptionMessage, ExceptionObject);
			}
			finally
			{
				//Immediately Close the Connection after use to free up resources
				privateOLEDBConnection.Close();
			}


		}

		public void AddParameter(string ParameterName, object Value , SQLDataType SQLType , int Size , ParameterDirection Direction)
		{

			SqlDbType buildDataType = new SqlDbType();
			Parameter buildParameter;

			switch (SQLType)
			{
				case SQLDataType.SQLString :
					buildDataType = SqlDbType.VarChar;
					break;
				case SQLDataType.SQLNVarchar :
					buildDataType = SqlDbType.NVarChar;
					break;
				case SQLDataType.SQLChar:
					buildDataType = SqlDbType.Char;
					break;
				case SQLDataType.SQLInteger:
					buildDataType = SqlDbType.Int;
					break;
				case SQLDataType.SQLBit:
					buildDataType = SqlDbType.Bit;
					break;
				case SQLDataType.SQLDateTime:
					buildDataType = SqlDbType.DateTime;
					break;
				case SQLDataType.SQLDecimal:
					buildDataType = SqlDbType.Decimal;
					break;
				case SQLDataType.SQLMoney:
					buildDataType = SqlDbType.Money;
					break;
				case SQLDataType.SQLImage:
					buildDataType = SqlDbType.Image;
					break;
			}

			buildParameter = new Parameter(ParameterName, Value, buildDataType, Size, Direction);
			privateParameterList.Add(buildParameter);

		}

        public class Parameter
		{
            public String ParameterName;
            public object ParameterValue;
            public SqlDbType ParameterDataType;
            public int ParameterSize;
            public ParameterDirection ParameterDirectionUsed;

            public Parameter(string passedParameterName, object passedValue , SqlDbType passedSQLType , int passedSize, ParameterDirection passedDirection)
			{

                ParameterName = passedParameterName;
                ParameterValue = passedValue;
                ParameterDataType = passedSQLType;
                ParameterSize = passedSize;
				ParameterDirectionUsed = passedDirection;
			}

		}


		private SqlParameter ConvertParameters(Parameter passedParameter)
		{

			SqlParameter returnSQLParameter = new SqlParameter();

			returnSQLParameter.ParameterName = passedParameter.ParameterName;
			returnSQLParameter.Value = passedParameter.ParameterValue;
			returnSQLParameter.SqlDbType = passedParameter.ParameterDataType;
			returnSQLParameter.Size = passedParameter.ParameterSize;
			returnSQLParameter.Direction = passedParameter.ParameterDirectionUsed;

			return returnSQLParameter;
		}


		public void ClearParameters()
		{
			try
			{
				privateParameterList.Clear();
			}
			catch (Exception parameterException )
			{
				throw new Exception(privateExceptionMessage + " Parameter List did not clear", parameterException);
			}
		}


		private void LogException(Exception ExceptionObject)
		{
			String EventLogMessage;           //this is the Message we will pass to the log

			try
			{
				//Create the Message to be passed from the exception 
				EventLogMessage = "An error occured in the following module: " + privateModuleName + " The Source was: " + ExceptionObject.Source + "\n With the Message: " + ExceptionObject.Message + "\n Stack Tace: " + ExceptionObject.StackTrace + "\n Target Site: " + ExceptionObject.TargetSite.ToString();
				//Define the Eventlog as an Application Log entry
				EventLog localEventLog = new EventLog("Application");
				//Write the entry to the Application Event log, using this Module's name, the message an make it an error message with an ID of 55
//				EventLog.WriteEntry(privateModuleName, EventLogMessage, EventLogEntryType.Error, 55);
			}
			catch (Exception EventLogException)
			{
				//'If the eventlog fails (like forgetting to make the ASPNET user account a member of the debugger group, we pass the error
				throw new Exception(privateExceptionMessage + " - EventLog Error: " + EventLogException.Message, EventLogException);
			}
		}


		private void ValidateSQLStatement(String SQLStatement)
		{
			//SQL Statement must be at least 10 characters ( "Select * form x" )
			if (SQLStatement.Length < 10 )
				throw new Exception(privateExceptionMessage + " The SQL Statement must be provided and at least 10 characters long");
		}
		private void ValidateSPStatement(String SQLStatement)
		{
		//SQL Statement must be at least 10 characters ( "Select * form x" )
		if (SQLStatement.Length < 2 )
			throw new Exception(privateExceptionMessage + " The Stored Procedure must be provided and at least 2 characters long");
		}
}
}
