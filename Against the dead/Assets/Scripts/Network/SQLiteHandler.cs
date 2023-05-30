using System;
using System.Data;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Data.SQLite;
using Random = UnityEngine.Random;

public class SqLiteHandler
{
    private const string DatabaseName = "database.db";

    public string _dbConnectionString;
    private SQLiteConnection _dbConnection;
    
    private static SqLiteHandler? _instance = null;
    

    public static SqLiteHandler Instance
    {
        get { return _instance ?? (_instance = new SqLiteHandler()); }
    }


    private SqLiteHandler()
    
    {
        _dbConnectionString = "Data source=" + Application.persistentDataPath + "/" + DatabaseName;
        _dbConnection = new SQLiteConnection(_dbConnectionString);
        _dbConnection.Open();
        CreateDatabase();
    }

    ~SqLiteHandler() 
    { 
        _dbConnection.Close();
    }

    private int CreateDatabase()
    {
        SQLiteCommand request = new SQLiteCommand(_dbConnection);
        
        request.CommandText = 
            $"CREATE TABLE IF NOT EXISTS users(id INT PRIMARY KEY NOT NULL, username VARCHAR(100), password varchar(64), inventory text, stats text)";
        
        return request.ExecuteNonQuery();
    }

    public SQLiteDataReader GetUser(string username)
    {
        SQLiteCommand request = new SQLiteCommand(_dbConnection);
        
        request.CommandText = $"SELECT * FROM users WHERE username = @username";
        
        request.Parameters.Add("@username", DbType.String);
        request.Parameters["@username"].Value = username;

        return request.ExecuteReader();
    }

    public SQLiteDataReader GetUser(int id)
    {
        SQLiteCommand request = new SQLiteCommand(_dbConnection);
        
        request.CommandText = $"SELECT * FROM users WHERE id = @ID";
        
        request.Parameters.Add("@ID", DbType.Int32);
        request.Parameters["@ID"].Value = id;

        return request.ExecuteReader();
    }

    public int UpdateUser(int id, string column, string inventory)
    {
        SQLiteCommand request = new SQLiteCommand(_dbConnection);
        
        request.CommandText = $"UPDATE users SET {column} = @inventory WHERE id = @id";
        
        request.Parameters.Add("@id", DbType.String);
        request.Parameters["@id"].Value = id;

        request.Parameters.Add("@inventory", DbType.String);
        request.Parameters["@inventory"].Value = inventory;
        
        return request.ExecuteNonQuery();
    }
    
    public int RegisterUser(string username, string password)
    {
        password = GetHash(password);
            
        SQLiteCommand request = new SQLiteCommand(_dbConnection);
        
        request.CommandText = $"INSERT INTO users VALUES(@id, @username, @password, " + // player identifier
                              "'{{\"list\" : []}}'," + // Inventory
                              "'{" + // Player stats
                              "\"health\" : 100," + 
                              "\"hunger\" : 100," +
                              "\"player\" : 0," +
                              "\"crawler\" : 0," +
                              "\"normal\" : 0," +
                              "}')";
        int id = GenerateId();
        request.Parameters.Add("@id", DbType.Int32);
        request.Parameters["@id"].Value = id;
        
        request.Parameters.Add("@username", DbType.String);
        request.Parameters["@username"].Value = username;
            
        request.Parameters.Add("@password", DbType.String);
        request.Parameters["@password"].Value = password;

        request.ExecuteNonQuery();
        return id;
    }

    private int GenerateId()
    {
        int id;
        do
        {
            id = (int)Random.Range(1, Int32.MaxValue);
        } while (GetUser(id).Read()); // Check if id isn't already used

        return id;
    }

    public bool CheckPassword(string username, string password)
    {
        password = GetHash(password);

        SQLiteCommand request = new SQLiteCommand(_dbConnection);

        request.CommandText = $"SELECT password FROM users where username = @username";

        request.Parameters.Add("@username", DbType.String);
        request.Parameters["@username"].Value = username;

        var reader = request.ExecuteReader();

        if (reader.Read())
        {
            return (string)reader["password"] == password;
        }

        return false;
    }

    private static string GetHash(string rawData)
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
