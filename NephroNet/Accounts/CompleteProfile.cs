using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NephroNet.Accounts
{
    public class CompleteProfile
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        public CompleteProfile(string in_profileId, string in_current_userId)
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            findCompleteProfile(in_profileId, in_current_userId);
        }
        protected void findCompleteProfile(string in_profileId, string in_current_userId)
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select user_firstname from Users where userId = '" + in_profileId + "' ";
            string profile_name = cmd.ExecuteScalar().ToString();
            cmd.CommandText = "select user_lastname from Users where userId = '" + in_profileId + "' ";
            profile_name = profile_name + " " + cmd.ExecuteScalar().ToString();
            connect.Close();
            //Set the name according to the profile name:
            name = profile_name;
        }
        public string name { get; set; }
        public string email { get; set; }
    }
}