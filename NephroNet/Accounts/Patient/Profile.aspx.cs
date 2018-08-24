using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Patient
{
    public partial class Profile : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string profileId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            profileId = Request.QueryString["id"];
            bool userIdExists = isUserCorrect();
            if (!userIdExists)
                goHome();
            showInformation();
        }
        protected string getInformation()
        {
            string info = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string userId = cmd.ExecuteScalar().ToString();
            ShortProfile shortProfile = new ShortProfile(profileId, userId);
            string shortProfileId = shortProfile.Id;
            string name = shortProfile.Name;
            string race = shortProfile.Race;
            string gender = shortProfile.Gender;
            string birthdate = shortProfile.Birthdate;
            string nationality = shortProfile.Nationality;
            int shortProfile_roleId = shortProfile.RoleId;
            ArrayList currentHealthConditions = shortProfile.CurrentHealthConditions;
            ArrayList currentTreatments = shortProfile.CurrentTreatments;
            string role_name = shortProfile.RoleName;
            string spaces = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            //loop through current health conditions:
            string str_currentHealthConditions = "";
            if (currentHealthConditions.Count > 0)
            {
                str_currentHealthConditions = "Current health conditions : <br/>";
                for (int i = 0; i < currentHealthConditions.Count; i++)
                {
                    str_currentHealthConditions += spaces + currentHealthConditions[i] + "<br/>";
                }
            }
            //loop through current Treatments:
            string str_currentTreatments = "";
            if (currentTreatments.Count > 0)
            {
                str_currentTreatments = "Current Treatments : <br/>";
                for (int i = 0; i < currentTreatments.Count; i++)
                {
                    str_currentTreatments += spaces + currentTreatments[i] + "<br/>";
                }
            }
            info =
                "Name: " + name + "<br />" +
                "Race: " + race + "<br />" +
                "Gender: " + gender + "<br />" +
                "Birthdate: " + birthdate + "<br />" +
                "Nationality: " + nationality + "<br />" +
                "Role: " + role_name + "<br />" +
                str_currentHealthConditions +
                str_currentTreatments;
            connect.Close();
            return info;
        }
        protected void showInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Get the current user's ID who is trying to access the profile:
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string current_userId = cmd.ExecuteScalar().ToString();
            connect.Close();
            //Display the information:
            lblShortProfileInformation.Text = getInformation();
        }
        protected bool isUserCorrect()
        {
            bool correct = true;
            CheckErrors errors = new CheckErrors();
            //check if id contains a special character:
            if (!errors.isDigit(profileId))
                correct = false;
            //check if id contains an id that does not exist in DB:
            else if (errors.ContainsSpecialChars(profileId))
                correct = false;
            if (correct)
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                //Count the existance of the user:
                cmd.CommandText = "select count(*) from Users where userId = '" + profileId + "' ";
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)//if count > 0, then the user ID exists in DB.
                {
                    //Get the current user's ID who is trying to access the profile:
                    cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
                    string current_userId = cmd.ExecuteScalar().ToString();
                    //Maybe later use the current user's ID to check if the current user has access to view the selected profile.
                }
                else
                    correct = false; // means that the user ID does not exists in DB.
                connect.Close();
            }
            return correct;
        }
        protected void goHome()
        {
            Response.Redirect("Home.aspx");
        }
        protected void initialPageAccess()
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            getSession();
            CheckPatientSession session = new CheckPatientSession();
            bool correctSession = session.sessionIsCorrect(username, roleId, token);
            if (!correctSession)
                clearSession();
        }
        protected void clearSession()
        {

            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/");
        }
        protected void addSession()
        {
            Session.Add("username", username);
            Session.Add("roleId", roleId);
            Session.Add("loginId", loginId);
            Session.Add("token", token);
        }
        protected void getSession()
        {
            username = (string)(Session["username"]);
            roleId = (string)(Session["roleId"]);
            loginId = (string)(Session["loginId"]);
            token = (string)(Session["token"]);
        }
    }
}