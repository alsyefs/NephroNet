using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Admin
{
    public partial class Search : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        protected void Page_Load(object sender, EventArgs e)
        {
            initialAccess();
            createSomeTable();
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
        protected void initialAccess()
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
        protected void hideEverything()
        {
            lblResultsMessage.Visible = false;
            lblErrorMessage.Visible = false;
        }
        protected void createSomeTable()
        {
            hideEverything();
            bool correct = checkInput();
            if (correct)
            {
                //Count the results:
                int count = countResults();
                //if no results, show a message:
                if (count == 0)
                {
                    lblResultsMessage.Visible = true;
                }
                else
                {
                    lblResultsMessage.Visible = false;
                    //call a method to create a table for the selected criteria:
                    if (drpSearch.SelectedIndex == 1)//Searching for topic titles
                        createTopicsTable(count);
                    else if (drpSearch.SelectedIndex == 2)//Searching for users' names
                        createUsersTable(count);
                    else if (drpSearch.SelectedIndex == 3)//Searching for messages
                        createMessagesTable(count);
                    else if (drpSearch.SelectedIndex == 4)//Searching for everything; topics, users, and messages
                        createEverythingTable(count);
                }
            }
            hideEverything();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hideEverything();
            bool correct = checkInput();
            if (correct)
            {
                //Count the results:
                int count = countResults();
                //if no results, show a message:
                if (count == 0)
                {
                    lblResultsMessage.Visible = true;
                }
                else
                {
                    lblResultsMessage.Visible = false;
                    //call a method to create a table for the selected criteria:
                    if (drpSearch.SelectedIndex == 1)//Searching for topic titles
                        createTopicsTable(count);
                    else if (drpSearch.SelectedIndex == 2)//Searching for users' names
                        createUsersTable(count);
                    else if (drpSearch.SelectedIndex == 3)//Searching for messages
                        createMessagesTable(count);
                    else if (drpSearch.SelectedIndex == 4)//Searching for everything; topics, users, and messages
                        createEverythingTable(count);
                }
            }
        }
        protected int countResults()
        {
            int count = 0;
            string searchString = txtSearch.Text.Replace("'", "''");
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            if(drpSearch.SelectedIndex == 1)//Searching for topic titles
            {
                cmd.CommandText = "select count(*) from topics where topic_title like '%"+ searchString + "%' and topic_isDeleted = 0 and topic_isDenied = 0 and topic_isApproved = 1 ";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else if(drpSearch.SelectedIndex == 2)//Searching for users' names
            {
                cmd.CommandText = "select count(*) from users where WHERE (user_firstname+ ' ' +user_lastname) like '%"+ searchString + "%' ";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else if (drpSearch.SelectedIndex == 3)//Searching for messages
            {
                cmd.CommandText = "select * from entries where entry_text like '%" + searchString + "%' ";
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else if (drpSearch.SelectedIndex == 4)//Searching for everything; topics, users, and messages
            {
                cmd.CommandText = "select count(*) from topics where topic_title like '%" + searchString + "%' and topic_isDeleted = 0 and topic_isDenied = 0 and topic_isApproved = 1 ";
                count = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "select count(*) from users where WHERE (user_firstname+ ' ' +user_lastname) like '%" + searchString + "%' ";
                count += Convert.ToInt32(cmd.ExecuteScalar());
                cmd.CommandText = "select * from entries where entry_text like '%" + searchString + "%' ";
                count += Convert.ToInt32(cmd.ExecuteScalar());
            }
            connect.Close();
            return count;
        }
        protected bool checkInput()
        {
            bool correct = true;
            lblErrorMessage.Text = "";
            //check if search text is blank:
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                correct = false;
                lblErrorMessage.Text += "Please type something in the search text field.<br/>";
            }
            //if input has one quotation, replace it with a double quotaion to avoid SQL errors:
            txtSearch.Text = txtSearch.Text.Replace("'", "''");
            //check if no criteria was selected:
            if(drpSearch.SelectedIndex == 0)
            {
                correct = false;
                lblErrorMessage.Text += "Please select a search criteria.<br/>";
            }
            if (!correct)
                lblErrorMessage.Visible = true;
            else
                lblErrorMessage.Visible = false;
            return correct;
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
        protected void drpSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void grdResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdResults.PageIndex = e.NewPageIndex;
            grdResults.DataBind();
            //Hide the header called "ID":
            //grdResults.HeaderRow.Cells[1].Visible = false;
            //Hide IDs column and content which are located in column index 1:
            //for (int i = 0; i < grdResults.Rows.Count; i++)
            //{
            //    grdResults.Rows[i].Cells[1].Visible = false;
            //}
        }
        protected void createTopicsTable(int count)
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Creator", typeof(string));
            string id = "", title = "", type = "", creator = "", time = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            for (int i = 1; i <= count; i++)
            {
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_time] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                time = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                //dt.Rows.Add(id, Layouts.getTimeFormat(time), title, type, creator);
                dt.Rows.Add(title, Layouts.getTimeFormat(time), type, creator);
            }
            grdResults.DataSource = dt;
            grdResults.DataBind();
            //grdTopics.AutoGenerateColumns = true;
            //Hide the header called "ID":
            //grdResults.HeaderRow.Cells[1].Visible = false;
            //Hide IDs column and content which are located in column index 1:
            //for (int i = 0; i < grdResults.Rows.Count; i++)
            //{
            //    grdResults.Rows[i].Cells[1].Visible = false;
            //}
            for (int row = 0; row < grdResults.Rows.Count; row++)
            {
                int i = (row + 1);
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] FROM [Topics] where topicId = " + id + " ";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                HyperLink creatorLink = new HyperLink();
                creatorLink.Text = creator + " ";
                creatorLink.NavigateUrl = "Profile.aspx?id=" + creatorId;
                grdResults.Rows[row].Cells[3].Controls.Add(creatorLink);
                HyperLink topicLink = new HyperLink();
                topicLink.Text = title + " ";
                topicLink.NavigateUrl = "ViewTopic.aspx?id=" + id;
                grdResults.Rows[row].Cells[0].Controls.Add(topicLink);
            }
            connect.Close();
        }
        protected void createUsersTable(int count)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Creator", typeof(string));
            string id = "", title = "", type = "", creator = "", time = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            for (int i = 1; i <= count; i++)
            {
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_time] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                time = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                dt.Rows.Add(id, Layouts.getTimeFormat(time), title, type, creator);
            }
            grdResults.DataSource = dt;
            grdResults.DataBind();
            //grdTopics.AutoGenerateColumns = true;
            //Hide the header called "ID":
            grdResults.HeaderRow.Cells[1].Visible = false;
            //Hide IDs column and content which are located in column index 1:
            for (int i = 0; i < grdResults.Rows.Count; i++)
            {
                grdResults.Rows[i].Cells[1].Visible = false;
            }
            for (int row = 0; row < grdResults.Rows.Count; row++)
            {
                id = grdResults.Rows[row].Cells[1].Text;
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] FROM [Topics] where topicId = " + id + " ";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                HyperLink creatorLink = new HyperLink();
                creatorLink.Text = creator + " ";
                creatorLink.NavigateUrl = "Profile.aspx?id=" + creatorId;
                grdResults.Rows[row].Cells[5].Controls.Add(creatorLink);
            }
            connect.Close();
        }
        protected void createMessagesTable(int count)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Creator", typeof(string));
            string id = "", title = "", type = "", creator = "", time = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            for (int i = 1; i <= count; i++)
            {
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_time] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                time = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                dt.Rows.Add(id, Layouts.getTimeFormat(time), title, type, creator);
            }
            grdResults.DataSource = dt;
            grdResults.DataBind();
            //grdTopics.AutoGenerateColumns = true;
            //Hide the header called "ID":
            grdResults.HeaderRow.Cells[1].Visible = false;
            //Hide IDs column and content which are located in column index 1:
            for (int i = 0; i < grdResults.Rows.Count; i++)
            {
                grdResults.Rows[i].Cells[1].Visible = false;
            }
            for (int row = 0; row < grdResults.Rows.Count; row++)
            {
                id = grdResults.Rows[row].Cells[1].Text;
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] FROM [Topics] where topicId = " + id + " ";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                HyperLink creatorLink = new HyperLink();
                creatorLink.Text = creator + " ";
                creatorLink.NavigateUrl = "Profile.aspx?id=" + creatorId;
                grdResults.Rows[row].Cells[5].Controls.Add(creatorLink);
            }
            connect.Close();
        }
        protected void createEverythingTable(int count)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Creator", typeof(string));
            string id = "", title = "", type = "", creator = "", time = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            for (int i = 1; i <= count; i++)
            {
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_time] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                time = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                dt.Rows.Add(id, Layouts.getTimeFormat(time), title, type, creator);
            }
            grdResults.DataSource = dt;
            grdResults.DataBind();
            //grdTopics.AutoGenerateColumns = true;
            //Hide the header called "ID":
            grdResults.HeaderRow.Cells[1].Visible = false;
            //Hide IDs column and content which are located in column index 1:
            for (int i = 0; i < grdResults.Rows.Count; i++)
            {
                grdResults.Rows[i].Cells[1].Visible = false;
            }
            for (int row = 0; row < grdResults.Rows.Count; row++)
            {
                id = grdResults.Rows[row].Cells[1].Text;
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] FROM [Topics] where topicId = " + id + " ";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                HyperLink creatorLink = new HyperLink();
                creatorLink.Text = creator + " ";
                creatorLink.NavigateUrl = "Profile.aspx?id=" + creatorId;
                grdResults.Rows[row].Cells[5].Controls.Add(creatorLink);
            }
            connect.Close();
        }
    }
}