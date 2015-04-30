using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineRegistration_app.BusinessLayer
{
    public class ClassInfo
    {
        public static string Name { get { return "Class"; } }

        private string tinyDateFormat = "MM/dd";
        private string shortDateFormat = "MM/dd/yyyy";

        private DateTime startdate;
        private List<DateTime> skipdatearray, mandatorydatearray;

        public ClassInfo()
        {
            ID = 0;
            startdate = EndDate = SkipDate = DateTime.MinValue;
            skipdatearray = new List<DateTime>();
            mandatorydatearray = new List<DateTime>();
            Days = Time = Area = "";
            Filled = Cancelled = false;
        }

        public int ID { get; set; }

        public DateTime StartDate
        {
            get { return this.startdate; }
            set { this.startdate = value; }
        }

        public string Start { get { return this.startdate.ToString(tinyDateFormat); } }

        public string StartShortDate { get { return this.startdate.ToShortDateString(); } }

        public DateTime EndDate { get; set; }

        public string End { get { return this.EndDate.ToString(tinyDateFormat); } }

        public string EndShortDate { get { return this.EndDate.ToShortDateString(); } }

        public DateTime SkipDate { get; set; }

        public string SkipShortDate
        {
            get
            {
                if (this.SkipDate.Equals(DateTime.MinValue)) return "";
                return this.SkipDate.ToString(shortDateFormat);
            }
        }

        public string Skip
        {
            get
            {
                if (this.SkipDate.Equals(DateTime.MinValue)) return "";
                return this.SkipDate.ToString(tinyDateFormat);
            }
        }

        /// <summary>
        /// Combines SkipShortDate and SkipDateArray into a single string value
        /// </summary>
        public string SkipDates { get { return this.combineExtraDates(this.Skip, this.skipdatearray); } }

        /// <summary>
        /// Combines SkipShortDate and SkipDateArray into a single string value for Javascript usage
        /// </summary>
        public string SkipDatesJS { get { return combineExtraDates(this.Skip, this.skipdatearray, ";"); } }

        public string SkipDateArray
        {
            get { return ExtraDateArrayString(this.skipdatearray, ";"); }
            set { this.SkipDate = setExtraDateArray(ref this.skipdatearray, this.SkipDate, value); }
        }

        public DateTime MandatoryDate { get; set; }

        public string MandatoryShortDate
        {
            get
            {
                if (this.MandatoryDate.Equals(DateTime.MinValue)) return "";
                return this.MandatoryDate.ToString(shortDateFormat);
            }
        }

        public string Mandatory
        {
            get
            {
                if (this.MandatoryDate.Equals(DateTime.MinValue)) return "";
                return this.MandatoryDate.ToString(tinyDateFormat);
            }
        }

        /// <summary>
        /// Combines MandatoryShortDate and MandatoryDateArray into a single string value
        /// </summary>
        public string MandatoryDates { get { return combineExtraDates(this.Mandatory, this.mandatorydatearray); } }

        /// <summary>
        /// Combines MandatoryShortDate and MandatoryDateArray into a single string value for Javascript usage
        /// </summary>
        public string MandatoryDatesJS { get { return combineExtraDates(this.Mandatory, this.mandatorydatearray, ";"); } }

        public string MandatoryDateArray
        {
            get { return ExtraDateArrayString(this.mandatorydatearray, ";"); }
            set { this.MandatoryDate = setExtraDateArray(ref this.mandatorydatearray, this.MandatoryDate, value); }
        }

        /// <summary>
        /// Combines the first extra date with an extra date array into a single string value
        /// </summary>
        private string combineExtraDates(string firstExtraDate, List<DateTime> extraDateArray, string separator = ", ")
        {
            string retVal = firstExtraDate;

            foreach (DateTime sDate in extraDateArray)
                retVal += (!string.IsNullOrEmpty(retVal) ? separator : "") + sDate.ToString(tinyDateFormat);

            return retVal;
        }

        /// <summary>
        /// Sets the extra date array (arrExtraDates) values based on value.
        /// Always returns extraDate value.
        /// </summary>
        /// <param name="arrExtraDates">Array of DateTime values to set</param>
        /// <param name="extraDate">The initial date from the array. Always returned.</param>
        /// <param name="value">Array values to set. Passed as a semi-colon delimited string.</param>
        /// <returns></returns>
        private DateTime setExtraDateArray(ref List<DateTime> arrExtraDates, DateTime extraDate, string value)
        {
            arrExtraDates.Clear();

            // Make sure value is null/empty.
            if (string.IsNullOrEmpty(value)) return extraDate;

            // Format the skip date arrays
            string[] arrDates = value.Split(';');
            foreach (string strDate in arrDates)
            {
                DateTime nDate;
                if (DateTime.TryParse(strDate, out nDate) && !arrExtraDates.Contains(nDate))
                    arrExtraDates.Add(nDate);
            }

            arrExtraDates.Sort((x, y) => x.CompareTo(y));

            if (extraDate.Equals(DateTime.MinValue))
            {
                 extraDate = arrExtraDates[0];
                // Remove the first element since it's being used set as extraDate
                arrExtraDates.RemoveAt(0);
            }

            return extraDate;
        }

        public string ExtraDateArrayString(List<DateTime> arrExtraDates, string separator) { return ExtraDateArrayString(arrExtraDates, separator, "MM/dd/yyyy"); }
        public string ExtraDateArrayString(List<DateTime> arrExtraDates, string separator, string format)
        {
            string retVal = "";

            foreach (DateTime sDate in arrExtraDates)
                retVal += (!string.IsNullOrEmpty(retVal) ? separator : "") + sDate.ToString(format);

            return retVal;
        }

        public string Days { get; set; }

        public string Time { get; set; }

        public string Area { get; set; }

        public bool Filled { get; set; }

        public bool Cancelled { get; set; }

        public string XPathAddress { get; set; }
    }
}