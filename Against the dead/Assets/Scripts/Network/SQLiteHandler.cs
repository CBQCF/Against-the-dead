using System;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class SqLiteHandler
{
    private const string DatabaseName = "database.db";

    public string _dbConnectionString;
    private IDbConnection _dbConnection;
    
    private static SqLiteHandler? _instance = null;
    

    public static SqLiteHandler Instance
    {
        get { return _instance ?? (_instance = new SqLiteHandler()); }
    }
    
    private SqLiteHandler()
    
        {
            _dbConnectionString = "URI=file:" + Application.persistentDataPath + "/" + DatabaseName;
            _dbConnection = new SqliteConnection(_dbConnectionString);
            _dbConnection.Open();
            CreateDatabase();
        }

        ~SqLiteHandler()
        {
            _dbConnection.Close();
        }

        private IDataReader CreateDatabase()
        {
            IDbCommand dbcmd = _dbConnection.CreateCommand();
            dbcmd.CommandText =
                "CREATE TABLE IF NOT EXISTS users(" +
                "id INT PRIMARY KEY NOT NULL," +
                "username VARCHAR(100)," +
                "password varchar(64)," +
                "inventory text," +
                "stats text)";
                IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public IDataReader GetUser(string username)
        {
            IDbCommand dbcmd = _dbConnection.CreateCommand();
            dbcmd.CommandText =
                $"SELECT * FROM users WHERE username = '{username}'";
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        public IDataReader GetUser(int id)
        {
            IDbCommand dbcmd = _dbConnection.CreateCommand();
            dbcmd.CommandText =
                $"SELECT * FROM users WHERE id = {id}";
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }
        
        public IDataReader RegisterUser(string username, string password)
        {
            password = GetHash(password);
            IDbCommand dbcmd = _dbConnection.CreateCommand();
            dbcmd.CommandText =
                "INSERT INTO users VALUES(" +
                $"{GenerateId()}," +
                $"'{username}'," +
                $"'{password}'," + 
                "'[]'," +
                "'{" +
                "\"health\" : 0," + 
                "\"hunger\" : 0," +
                "\"player\" : 0," +
                "\"crawler\" : 0," +
                "\"normal\" : 0," +
                "}')";
            IDataReader reader = dbcmd.ExecuteReader();
            return reader;
        }

        private int GenerateId()
        {
            int id;
            do
            {
                id = (int)Random.Range(1, Int32.MaxValue);
            } while (GetUser(id).Read());

            return id;
        }
        
        public bool CheckPassword(string username, string password)
        {
            password = GetHash(password);
            IDbCommand dbcmd = _dbConnection.CreateCommand();
            dbcmd.CommandText =
                $"SELECT password FROM users where username = '{username}'";
            IDataReader reader = dbcmd.ExecuteReader();
            reader.Read();
            return (string)reader["password"] == password;
        }

        public static string GetHash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  
                return builder.ToString();  
            }  
        }
        
        
        
        public void Close (){
			_dbConnection.Close ();
		}
}
