using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineRegistration_app.BusinessLayer
{
    /// <summary>
    /// Handles the XML Data of Schedule Series (by month).
    /// </summary>
    public class SeriesInfo
    {
        public static string Name { get { return "Series"; } }

        private string title;
        private bool customTitle;
        private DateTime date;
        private bool visible;
        private string xpathaddress;

        /// <summary>
        /// Class Initializer
        /// </summary>
        public SeriesInfo()
        {
            title = xpathaddress = "";
            date = DateTime.MinValue;
            visible = true;
        }

        /// <summary>
        /// Full date for the Series.
        /// </summary>
        public DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        /// <summary>
        /// Month part for the Series.
        /// </summary>
        public int Month
        { get { return this.date.Month; } }

        /// <summary>
        /// Month name for the Series (January, February, etc).
        /// </summary>
        public string MonthName
        { get { return this.date.ToString("MMMM"); } }

        /// <summary>
        /// Month and Year parts combined as a string for the Series.
        /// </summary>
        public string MonthAndYear
        { get { return MonthName + " " + Year.ToString(); } }

        /// <summary>
        /// Combines Title and MonthAndYear together, or returns just Title if CustomTitle is set.
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                string rTitle = Title;

                if (!CustomTitle) rTitle += " " + MonthAndYear;

                return rTitle;
            }
        }

        /// <summary>
        /// When set to True, the Series DisplayTitle will only return what is set for Title,
        /// otherwise, it will return Title and MonthAndYear combined.
        /// </summary>
        public bool CustomTitle
        {
            get { return this.customTitle; }
            set { this.customTitle = value; }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public int Year
        { get { return this.date.Year; } }

        public string XPathAddress
        {
            get { return this.xpathaddress; }
            set { this.xpathaddress = value; }
        }
    }
}