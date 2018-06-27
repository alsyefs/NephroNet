using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NephroNet
{
    public class Configuration
    {
        //The below is Saleh's personal connection string:
        static string connectionString = "data source=R14\\SALEH;initial catalog=NephroNetDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
        public string getConnectionString()
        {
            SqlConnection connect = new SqlConnection(connectionString);
            return connectionString;            
        }
        //Note that Gmail service might not allow to send emails, to solve that, you need to login to the below email 
        //regulary using the computer then check and verify the activity. Afterwards, the emailing service will work in the application.
        //This is a restriction from Gmail.
        string email = "NephroNet2018@gmail.com";
        //The password cannot be hashed as password are already in Gmail database; therefore, sending a hashed password will result rehashing the 
        // hash and password will be wrong. to verify password, Gmail does: hash(password) = hashedPassword
        //if we send the hashed password, Gmail would do: hash(hashedPassword) != hashedPassword
        string password = "Saleh.Alsyefi1988";
        public string getEmail()
        {
            return email;
        }
        public string getPassword()
        {
            return password;
        }
    }
}