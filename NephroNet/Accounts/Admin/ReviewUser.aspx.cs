using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class ReviewUser : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        string registerId = "";
        //Globals for "Users" table:
        string g_firstName, g_lastName, g_email, g_city, g_state, g_zip, g_address, g_phone;
        //Globals for "Logins" table:
        int g_roleId;
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            registerId = Request.QueryString["id"];
            showUserInformation();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addSession();
            Response.Redirect("ApproveUsers");
        }

        protected void showUserInformation()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //Check if the ID exists in the database:
            cmd.CommandText = "select count(*) from registrations where registerId = '"+registerId+"' ";
            int countUser = Convert.ToInt32(cmd.ExecuteScalar());
            if(countUser > 0)//if ID exists, countUser = 1
            {
                //Get first name:
                cmd.CommandText = "select register_firstname from [Registrations] where [registerId] = '"+registerId+"' ";
                string firstName = cmd.ExecuteScalar().ToString();
                //Get last name and add it to the first name:
                cmd.CommandText = "select register_lastname from [Registrations] where [registerId] = '" + registerId + "' ";
                string lastName = " " + cmd.ExecuteScalar().ToString();
                //Get email:
                cmd.CommandText = "select register_email from [Registrations] where [registerId] = '" + registerId + "' ";
                string email = cmd.ExecuteScalar().ToString();
                //Get city:
                cmd.CommandText = "select register_city from [Registrations] where [registerId] = '" + registerId + "' ";
                string city = cmd.ExecuteScalar().ToString();
                //Get state:
                cmd.CommandText = "select register_state from [Registrations] where [registerId] = '" + registerId + "' ";
                string state = cmd.ExecuteScalar().ToString();
                //Get zip code:
                cmd.CommandText = "select register_zip from [Registrations] where [registerId] = '" + registerId + "' ";
                string zip = cmd.ExecuteScalar().ToString();
                //Get address:
                cmd.CommandText = "select register_address from [Registrations] where [registerId] = '" + registerId + "' ";
                string address = cmd.ExecuteScalar().ToString();
                //Get role ID as int:
                cmd.CommandText = "select register_roleId from [Registrations] where [registerId] = '" + registerId + "' ";
                int int_roleId = Convert.ToInt32(cmd.ExecuteScalar());
                //Convert role ID to string:
                string role = "";
                if (int_roleId == 1)
                    role = "Admin";
                else if (int_roleId == 2)
                    role = "Physician";
                else
                    role = "Patient";
                //Get phone:
                cmd.CommandText = "select register_phone from [Registrations] where [registerId] = '" + registerId + "' ";
                string phone = cmd.ExecuteScalar().ToString();
                //Create an informative message containing all information for the selected user:
                lblUserInformation.Text = "Name: " + firstName + " " + lastName +"<br />"+
                    "Email: " + email + "<br />"+
                    "Address: " + address + "<br />" +
                    "City: " + city + ", State: " + state + "<br />" +
                    "Zip code: " + zip + "<br />" +
                    "Phone#: " + phone + "<br />" +
                    "Role: " + role;
                lblUserInformation.Visible = true;
                //Copy values to globals:
                g_firstName = firstName; g_lastName = lastName; g_email = email; g_city = city; g_state = state; g_zip = zip; g_address = address; g_phone=phone;g_roleId = int_roleId;
            }
            else
            {
                addSession();
                Response.Redirect("ApproveUsers");
            }
            connect.Close();
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
            lblAlerts.Text = "(" + session.countTotalAlerts() + ")";
        }
        protected string createUsername()
        {
            string generatedUsername = "";
            generatedUsername = g_firstName + g_lastName + g_roleId + registerId;
            generatedUsername = generatedUsername.Replace(" ", "");
            //Make sure the new username doesn't match another username in the system:
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "select count(*) from Logins where login_username like '"+generatedUsername+"' ";
            int countDuplicateUsernames = Convert.ToInt32(cmd.ExecuteScalar());
            connect.Close();
            if(countDuplicateUsernames > 0)
            {
                Random rnd = new Random();
                int addUniqueness = rnd.Next(1, 999);
                generatedUsername = generatedUsername + addUniqueness;
            }
            return generatedUsername;
        }
        protected string createPassword()
        {
            string generatedPassword = "";
            generatedPassword = "123";
            return generatedPassword;
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //Hide the success message:
            lblMessage.Visible = false;
            //Create a new unique username:
            string newUsername = createUsername();
            //Create an initial password:
            string newPassword = createPassword();
            //Hash the password:
            string hashedPassword = Encryption.hash(newPassword);
            //Set login_attempts = 0, login_securityQuestionsAttempts = 0, login_initial = 1 and login_isActive = 1: (1 in bit = true)
            //Store the previous information into the table "Logins":
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "insert into Logins (login_username, login_password, roleId, login_attempts, login_securityQuestionsAttempts, login_initial, login_isActive) values " +
                "('"+newUsername+"', '"+ hashedPassword + "', '"+g_roleId+"', 0, 0, 1, 1)";
            cmd.ExecuteScalar();
            //Get the loginID of the user just created using the username:
            cmd.CommandText = "select loginId from Logins where login_username like '"+newUsername+"' ";
            string newLoginId = cmd.ExecuteScalar().ToString();
            //Store the user's information into the "Users" table:
            cmd.CommandText = "insert into Users (user_firstname, user_lastname, user_email, user_city, user_state, user_zip, user_address, user_phone, loginId) values " +
                "('"+g_firstName+"', '"+g_lastName+"', '"+g_email+"', '"+g_city+"', '"+g_state+"', '"+g_zip+"', '"+g_address+"', '"+g_phone+"', '"+newLoginId+"') ";
            cmd.ExecuteScalar();

            connect.Close();
            //Create an email message to be sent:
            string emailMessage = "Hello "+g_firstName + " " + g_lastName + ",\n\n"+
                "This email is to inform you that your account has been approved for NephroNet. To access the site, you need the following information:\n"+
                "username: " + newUsername + "\n"+
                "password: " + newPassword + "\n"+
                "Remeber, your provided is a temporary password and you must change it once you login to the site.\n\n"+
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification the user using the entered email:
            Email emailClass = new Email();
            emailClass.sendEmail(g_email, emailMessage);
            //Display a success message:
            lblMessage.Visible = true;
            lblMessage.Text = "The selected user has been successfully approved and the new information has been emailed to the user!";
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
            //Delete user information from "Registrations" table:
            connect.Open();
            cmd.CommandText = "delete from [Registrations] where registerId = '" + registerId + "' ";
            cmd.ExecuteScalar();
            connect.Close();
        }
        protected void hideApproveDeny()
        {
            btnApprove.Visible = false;
            btnDeny.Visible = false;
        }
        protected void btnDeny_Click(object sender, EventArgs e)
        {
            //Delete user information from "Registrations" table:
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            cmd.CommandText = "delete from [Registrations] where registerId = '"+registerId+"' ";
            cmd.ExecuteScalar();
            connect.Close();
            //Create an email message to be sent:
            string emailMessage = "Hello " + g_firstName + " " + g_lastName + ",\n\n" +
                "This email is to inform you that your account has been denied for NephroNet. For more information, please contact the support.\n\n" +
                "Best regards,\nNephroNet Support\nNephroNet2018@gmail.com";
            //Send an email notification the user using the entered email:
            Email emailClass = new Email();
            emailClass.sendEmail(g_email, emailMessage);
            //Show in a message that the user was denied:
            lblMessage.Visible = true;
            lblMessage.Text = "The selected user has been successfully denied, emailed and removed from the list of applied users!";
            //Hide "Approve" and "Deny" buttons:
            hideApproveDeny();
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