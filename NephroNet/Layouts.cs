using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace NephroNet
{
    public class Layouts
    {
        public static string postHeader(string creator, string topic_type, string topic_title, string topic_time, string topic_description, string imagesHTML)
        {
            string background_color = "style = \"background-color:#CECECE; width: 100%;\"";
            string header = "<p " + background_color + " >" +
            //"______________________________________________________________________________________________________________________________<br />" +
            "Creator: " + creator + "<br />" +
            "Type: " + topic_type + "<br />" +
            "Title: " + topic_title + "<br />" +
            "Time: " + getTimeFormat(topic_time) + "<br />" +
            //"Terminated?: " + topic_isTerminated + "<br />" +
            "Description: \"" + topic_description + "\"<br />" +
            imagesHTML + "<br />" +
            //"______________________________________________________________________________________________________________________________"+
            "</p>";
            return header;
        }

        public static string postMessage(int i, string creator_name, string entry_time, string entry_text, string imagesHtml, 
            string entry_creatorId, string topic_creatorId, string userId, string entryId)
        {
            string deleteCommand = "";
            //Check if the user viewing has an entry he/she created, or if the current user viewing is the topic creator::
            if (entry_creatorId.Equals(userId) || topic_creatorId.Equals(userId))
                deleteCommand = "<a href=\"DeleteEntry.aspx?entryId=" + entryId + "\"> Remove Entry " + i + "</a> <br />";
            string background_color = "";
            if (i % 2 == 0)
                background_color = "style = \"background-color:#CCCC99; width: 100%;\"";
            else
                background_color = "style = \"background-color:#F7F7DE; width: 100%;\"";
            string message = "<p " + background_color + " >Message " + i + " - added by " + creator_name + " on " + getTimeFormat(entry_time) + "<br />" +
                    entry_text + "<br /> " +
                        imagesHtml +
                        "<br /> " +
                    deleteCommand +
                    "--------------------------------------------------------------------------------------------" +
                        "<br /></p>";
            return message;
        }
        public static string getTimeFormat(string originalTime)
        {
            string format = "";
            string test = format;
            DateTime dateTime = Convert.ToDateTime(originalTime);
            //DateTime dateTime = DateTime.ParseExact(originalTime, "mmddyyyyHH:mm:ss", CultureInfo.CurrentCulture);
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
            string day = dateTime.Day.ToString("00");
            string year = dateTime.Year.ToString("0000");
            string hours = dateTime.Hour.ToString("00");
            //string minutes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Minute);
            string minutes = dateTime.Minute.ToString("00");
            string seconds = dateTime.Second.ToString("00");
            //string milliseconds = dateTime.Millisecond.ToString();
            //Get AM/PM:
            string dayOrNight = "AM";
            int int_hours = Convert.ToInt32(hours);
            if (int_hours > 12)
                dayOrNight = "PM";
            //Change the hour format to 12-hour-format:
            if (int_hours == 0)
                hours = "12";
            else if (int_hours > 12)
                hours = int_hours - 12 + "";
            format = month + " " + day + ", " + year + " " + hours + ":" + minutes + ":" + seconds + " " + dayOrNight;
            return format;
        }

    }
}