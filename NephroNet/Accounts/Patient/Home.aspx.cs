using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NephroNet.Accounts.Patient
{
    public partial class Home : System.Web.UI.Page
    {
        static string conn = "";
        SqlConnection connect = new SqlConnection(conn);
        string username, roleId, loginId, token;
        protected void Page_Load(object sender, EventArgs e)
        {
            initialPageAccess();
            int countNewTopics = getTotalApprovedTopics();
            if (countNewTopics > 0)
            {
                lblMessage.Visible = false;
                createTable(countNewTopics);
                //int pageNum = Convert.ToInt32(Request.QueryString["page"]);
                //createTableHtml(countNewTopics, pageNum);
            }
            else if (countNewTopics == 0)
            {
                lblMessage.Visible = true;
            }
        }
        protected void createTableHtml(int countTopics, int pageNum)
        {
            string table_start = "<table style=\"width: 100 %; \">";
            //string row_start = "<tr>";
            string row_end = "</tr>";
            string column_start = "<td>";
            string column_end = "</td>";
            string table_end = "</table>";
            string table = "", rows = "";
            int rowsPerPage = 2;
            int pages = countTopics / rowsPerPage;
            for (int i = 1 + (pageNum * rowsPerPage); i <= countTopics; i++)
            {
                connect.Open();
                SqlCommand cmd = connect.CreateCommand();
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string id = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                string creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                connect.Close();
                string color = "";
                if (i % 2 == 1)//change color for each odd numbered row
                {
                    color = "#F7F7DE";
                }
                string row = "<tr bgcolor=\"" + color + "\">" + column_start + i + column_end + column_start + "<a href = \"ViewTopic.aspx?id=" + id + "\"> Enter </a>" + column_end + column_start + title + column_end + column_start + type + column_end + column_start + creator + column_end + row_end;
                rows = rows + row;
                if (i % rowsPerPage == 0)//limit to 10 rows
                {
                    if (pageNum == 0)
                        pageNum = 1;
                    string pageNumbers = ""; //"<a href=\"Home\\?page=" + pageNum + "\">Next >></a>";
                    //Show current and previous pages up to 5 page numbers, then display the first page number followed by "...":
                    //for (int k = pages; k >= pageNum; k--)
                    //{
                    //    if (k > pageNum + 2)
                    //        pageNumbers = pageNumbers + "<a href=\"Home.aspx?page=" + k + "\"> " + k + " </a>";
                    //    if (k == pageNum && k <= pages + 2)
                    //    {
                    //        pageNumbers = pageNumbers + "....<a href=\"Home.aspx?page=" + k + "\"> " + k + " </a>";
                    //    }
                    //}
                    int tempPageNum = pageNum;
                    //Show current and next pages up to 5 page numbers, then display "..." followed by the last page number:
                    for (int k = pageNum; k <= pages; k++)
                    {
                        tempPageNum--;
                        if (pageNum > 1 && tempPageNum > 0)//(pageNum > 3 && tempPageNum > 0)
                        {
                            //pageNumbers = pageNumbers + "<a href=\"Home.aspx?page=" + 1 + "\"> 1 </a>...";
                            pageNumbers = pageNumbers + "<a href=\"Home.aspx?page=" + tempPageNum + "\"> " + tempPageNum + " </a>";
                        }
                        if (k < pageNum + 2)
                            pageNumbers = pageNumbers + "<a href=\"Home.aspx?page=" + k + "\"> " + k + " </a>";
                        if (k == pages && k >= pageNum + 2)
                        {
                            pageNumbers = pageNumbers + "....<a href=\"Home.aspx?page=" + k + "\"> " + k + " </a>";
                        }
                    }
                    rows = rows + "<tr bgcolor=\"" + color + "\">" + column_start + " " + column_end + column_start + "" + column_end + column_start + "" + column_end + column_start + pageNumbers + column_end + row_end;
                    break;
                }
            }
            string fontColor = "white";
            string header = "<tr bgcolor=\"#6B696B\">" +
                column_start + " " + column_end +
                column_start + " " + column_end +
                column_start + "<font color=\"" + fontColor + "\">Title</font>" + column_end +
                column_start + "<font color=\"" + fontColor + "\">Type</font>" + column_end +
                column_start + "<font color=\"" + fontColor + "\">Creator</font>" + column_end + row_end;
            table = table + table_start + header + rows + table_end;
            lblTable.Text = table;
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
            lblAlerts.Text = "(" + session.countTotalAlerts() + ")";
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
        protected void grdTopics_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTopics.PageIndex = e.NewPageIndex;
            grdTopics.DataBind();
        }
        protected void createTable(int count)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Creator", typeof(string));
            string id = "", title = "", type = "", creator = "";
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            for (int i = 1; i <= count; i++)
            {
                //Get the topic ID:
                cmd.CommandText = "select [topicId] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                id = cmd.ExecuteScalar().ToString();
                //Get title:
                cmd.CommandText = "select [topic_title] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                title = cmd.ExecuteScalar().ToString();
                //Get type:
                cmd.CommandText = "select [topic_type] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                type = cmd.ExecuteScalar().ToString();
                //Get creator's ID:
                cmd.CommandText = "select [topic_createdBy] from(SELECT rowNum = ROW_NUMBER() OVER(ORDER BY topicId ASC), * FROM [Topics] where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0) as t where rowNum = '" + i + "'";
                string creatorId = cmd.ExecuteScalar().ToString();
                //Get creator's name:
                cmd.CommandText = "select user_firstname from users where userId = '" + creatorId + "' ";
                creator = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select user_lastname from users where userId = '" + creatorId + "' ";
                creator = creator + " " + cmd.ExecuteScalar().ToString();
                dt.Rows.Add(id, title, type, creator);
            }
            connect.Close();
            grdTopics.DataSource = dt;
            grdTopics.DataBind();
        }
        protected int getTotalApprovedTopics()
        {
            connect.Open();
            SqlCommand cmd = connect.CreateCommand();
            //count the approved topics:
            cmd.CommandText = "select count(*) from Topics where topic_isApproved = 1 and topic_isDenied = 0 and topic_isTerminated = 0 and topic_isDeleted = 0";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            connect.Close();
            return count;
        }
    }
}