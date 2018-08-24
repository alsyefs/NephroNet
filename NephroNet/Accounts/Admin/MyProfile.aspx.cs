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
            lblShortProfileInfo.Text = getShortProfileInformation();
            lblCompleteProfileInfo.Text = getCompleteProfileInformation();
        }
        protected string getCompleteProfileInformation()
        {
            string info = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string userId = cmd.ExecuteScalar().ToString();
            CompleteProfile completeProfile = new CompleteProfile(userId);
            string completeProfileId = completeProfile.Id;
            string onDialysis = completeProfile.OnDialysis;
            string kidneyDisease = completeProfile.KidneyDisease;
            string issueDate = completeProfile.IssueStartDate;
            string bloodType = completeProfile.BloodType;
            string address = completeProfile.Address + "<br/>" + completeProfile.City + ", " + completeProfile.State + " " + completeProfile.Zip + "<br/>";
            
            List<Insurance> insurances = new List<Insurance>();
            insurances = completeProfile.Insurances;
            //insurances.Select(c => c.ID.ToList());
            string str_insurances = "";
            string insurance_address, insurance_city, insurance_companyName, insurance_completeProfileId, 
                insurance_country, insurance_email, insurance_groupId, insurance_id, insurance_memberId, insurance_phone1, insurance_phone2, insurance_state, insurance_zip;
            foreach(Insurance ins in insurances)
            {
                insurance_address = ins.Address;
                insurance_city = ins.City;
                insurance_companyName = ins.CompanyName;
                insurance_completeProfileId=  ins.CompleteProfileId;
                insurance_country = ins.Country;
                insurance_email = ins.Email;
                insurance_groupId = ins.GroupId;
                insurance_id =  ins.ID;
                insurance_memberId = ins.MemberId;
                insurance_phone1 = ins.Phone1;
                insurance_phone2 = ins.Phone2;
                insurance_state =  ins.State;
                insurance_zip =  ins.Zip;
                str_insurances += "Insurances: <br/>" +
                    "Insurance ID: " + insurance_id + "<br/>" +
                    "Member ID: " + insurance_memberId + "<br/>" +
                    "Group ID: " + insurance_groupId + "<br/>" +
                    "Insurance name: " + insurance_companyName + "<br/>" +
                    "Insurance phone1 : " + insurance_phone1 + "<br/>" +
                    "Insurance phone2: " + insurance_phone2 + "<br/>" +
                    "Insurance email: " + insurance_email + "<br/>" +
                    "Insurance address: " + insurance_address + "<br/>" + insurance_city + ", " + insurance_state + " " + insurance_zip + "<br/>" + insurance_country;
            }
            
            info =
                "<br/>Complete Profile Information: <br/>"+
                "On dialysis: " + onDialysis + "<br/>" +
                "Kidney disease stage: " + kidneyDisease + "<br/>" +
                "Health issue started on: " + issueDate + "<br/>" +
                "Blood type: " + bloodType + "<br/>" +
                "Address: " + address + "<br/>" +
                str_insurances + "<br/>";
            connect.Close();
            return info;
        }
        protected string getShortProfileInformation()
        {
            string info = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select userId from Users where loginId = '" + loginId + "' ";
            string userId = cmd.ExecuteScalar().ToString();
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