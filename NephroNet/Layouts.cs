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
			string background_color = "style = \"background-color:#CECECE; width: 100%; border-bottom: 6px solid black; border: 2px solid black; border-radius: 5px;\"";
			string header = "<div id=\"header\">" +
            "<div id=\"messageHead\">" +
            "&nbsp;\"" + topic_title + "\" " +
            "Created by <a href=\"profile.aspx?id=\">" + creator + " </a>" +
            "as a " + topic_type.ToLower() + " topic on " + getTimeFormat(topic_time) + "</div>" +
            "<div id=\"messageDescription\"><br/>" + topic_description + "<br /><br/>" +
			imagesHTML + "</div><br />" +
			"</div>";
			return header;
		}

		public static string postMessage(int i, string creator_name, string entry_time, string entry_text, string imagesHtml, 
			string entry_creatorId, string topic_creatorId, string userId, string entryId)
		{
			string deleteCommand = "";
            //Check if the user viewing has an entry he/she created, or if the current user viewing is the topic creator::
            if (entry_creatorId.Equals(userId) || topic_creatorId.Equals(userId))
                //deleteCommand = "&nbsp;<a href=\"DeleteEntry.aspx?entryId=" + entryId + "\">Remove Entry " + i + "</a><br />";
                deleteCommand = "&nbsp;<a onmousedown=\"OpenPopup('RemoveEntry.aspx?id=" + entryId + "')\">Remove Entry " + i + "</a><br/>";
			string background_color = "";
			if (i % 2 == 0)
				background_color = "style = \"background: rgb(255, 255, 255);background: rgba(203, 203, 152, 0.6);\"";
			//else
			//	background_color = "style = \"background-color:#F7F7DE; width: 100%; border: 2px solid black; border-radius: 5px;\"";
			string message = "<div id=\"message\" " + background_color + " ><div id=\"messageHead\">&nbsp;Message #" + i + " - added by " + 
                "<a href=\"profile.aspx?id=" + entry_creatorId + "\"> "    +creator_name + "</a>"+ 
                " on " + getTimeFormat(entry_time) + "</div> " +
                    "<div id=\"messageDescription\"><p><br/>" + entry_text + "</p><br /> " +
						imagesHtml +
                        "</div> " +
					deleteCommand +
                    "</div><br />";
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