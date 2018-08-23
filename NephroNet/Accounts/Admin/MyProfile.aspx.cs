using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class MyProfile : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            showInformation();
        }
        protected void initialPageAccess()
        {
            Configuration config = new Configuration();
            conn = config.getConnectionString();
            connect = new SqlConnection(conn);
            getSession();
            CheckAdminSession session = new CheckAdminSession();
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
        protected void showInformation()
        {
            lblInfo.Text = getInformation();
        }
        protected string getInformation()
        {
            string info = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string userId = cmd.ExecuteScalar().ToString();
            CompleteProfile completeProfile = new CompleteProfile(userId, userId);
            ShortProfile shortProfile = new ShortProfile(userId, userId);
            string shortProfileId = shortProfile.Id;
            string name = shortProfile.Name;
            string race = shortProfile.Race;
            string gender = shortProfile.Gender;
            string birthdate = shortProfile.Birthdate;
            string nationality = shortProfile.Nationality;
            int shortProfile_roleId = shortProfile.RoleId;
            ArrayList blockedUsers = shortProfile.BlockedUsers;
            ArrayList currentHealthConditions = shortProfile.CurrentHealthConditions;
            ArrayList currentTreatments = shortProfile.CurrentTreatments;
            string role_name = shortProfile.RoleName;
            string spaces = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            //loop through blocked users:
            string str_blockedUsers = "";
            if (blockedUsers.Count > 0)
            {
                str_blockedUsers = "Blocked users: <br/>";
                for (int i = 0; i < blockedUsers.Count; i++)
                {
                    str_blockedUsers += spaces + blockedUsers[i] + "<br/>";
                }
            }
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
                str_blockedUsers +
                str_currentHealthConditions +
                str_currentTreatments;
            connect.Close();
            return info;
        }
    }
}