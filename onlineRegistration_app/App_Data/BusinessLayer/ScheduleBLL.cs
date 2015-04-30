using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Collections;
using System.Text;
using System.Xml.Xsl;

namespace onlineRegistration_app.BusinessLayer
{
   public class ScheduleBLL
   {
      private Cache objCache = HttpContext.Current.Cache;
      private XElement root;
      private string filePath;
      private string error;
      private const string EXCEPTION_ROOT_NULL = "The XML documents root element is null.";
      private const string EXCEPTION_SERIES_EXISTS = "there is an existing series with the same Month and Year values.";
      private const string EXCEPTION_SERIES_NEW_EXISTS = "Cannot create new Series: " + EXCEPTION_SERIES_EXISTS;
      private const string EXCEPTION_SERIES_UPDATE_EXISTS = "Cannot update Series: " + EXCEPTION_SERIES_EXISTS;

      public ScheduleBLL()
      {
         this.error = "";
         this.filePath = HttpContext.Current.Server.MapPath("~/data/schedule.xml");
         this.root = getXElement;
      }

      /// <summary>
      /// Returns the entire XML document from Cache.
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
      public XElement getXElement
      {
         get
         {
            try
            {
               string cacheId = "Schedule_XML";

               if (objCache[cacheId] != null) return (XElement)objCache[cacheId];

               if (!File.Exists(this.filePath))
               {
                  XmlDocument xDoc = new XmlDocument();
                  // Save the xml declaration
                  XmlDeclaration xDec = xDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                  // Create root element
                  XmlElement rootNode = xDoc.CreateElement("Schedule");
                  xDoc.InsertBefore(xDec, xDoc.DocumentElement);
                  xDoc.AppendChild(rootNode);
                  xDoc.Save(this.filePath);
               }

               // Read the XML file into our XElement object.
               XElement xmlDoc = XElement.Load(this.filePath);

               // Setup a cache dependency on the file itself.
               using (CacheDependency xmlCacheDepend = new CacheDependency(this.filePath))
               {
                  // Store the XmlDocument object into Cache
                  objCache.Insert(cacheId, xmlDoc, xmlCacheDepend, Cache.NoAbsoluteExpiration, new TimeSpan(2, 0, 0, 0));
               }

               return xmlDoc;
            }
            catch (Exception ex) { this.formatError(ex); return null; }
         }
      }

      #region Helper Methods
      public string Error { get { return this.error; } }

      /// <summary>
      /// Generates an error message from an exception
      /// </summary>
      /// <param name="ex">Exception object</param>
      private void formatError(Exception ex) { this.error = ex.Message; }

      /// <summary>
      /// Converts the xAttribute value into a proper data type
      /// </summary>
      /// <typeparam name="T">Data type to convert into</typeparam>
      /// <param name="xAttribute">XML XAttribute</param>
      /// <returns></returns>
      private T getXAttributeValue<T>(XAttribute xAttribute)
      {
         string value = "";
         if (xAttribute != null) value = xAttribute.Value;
         return getValue<T>(value);
      }

      /// <summary>
      /// Converts the xElement value into a proper data type
      /// </summary>
      /// <typeparam name="T">Data type to convert into</typeparam>
      /// <param name="xAttribute">XML XElement</param>
      /// <returns></returns>
      private T getXElementValue<T>(XElement xElement)
      {
         string value = "";
         if (xElement != null) value = xElement.Value;
         return getValue<T>(value);
      }

      /// <summary>
      /// Converts a string value into another data type
      /// </summary>
      /// <typeparam name="T">Data type to convert into</typeparam>
      /// <param name="value">String value to be converted</param>
      /// <returns></returns>
      private T getValue<T>(string value)
      {
         T tValue = default(T);

         if (!string.IsNullOrEmpty(value))
         {
            if (typeof(T) == typeof(DateTime))
               tValue = (T)(object)getDateByString(value);
            else if (typeof(T) == typeof(bool))
               tValue = (T)(object)getBoolByString(value);
            else if (typeof(T) == typeof(string))
               tValue = (T)(object)getXMLString(value);
         }

         return tValue;
      }

      /// <summary>
      /// Converts a string into a boolean object
      /// </summary>
      /// <param name="value">String bool value</param>
      /// <returns></returns>
      private bool getBoolByString(string value)
      {
         bool tempBool;
         bool.TryParse(value, out tempBool);
         return tempBool;
      }

      /// <summary>
      /// Converts a string into a date object
      /// </summary>
      /// <param name="value">String date value</param>
      /// <returns></returns>
      private DateTime getDateByString(string value)
      {
         DateTime tempDate;
         if (!DateTime.TryParse(value, out tempDate))
            tempDate = DateTime.MinValue;

         return tempDate;
      }

      /// <summary>
      /// Checks to make sure the XML String value is not null or empty.
      /// If it is not then return the value, otherwise return an empty string.
      /// </summary>
      /// <param name="value">String value from XML node</param>
      /// <returns></returns>
      private string getXMLString(string value)
      {
         if (!string.IsNullOrEmpty(value)) return value;
         return string.Empty;
      }

      private string getXPathAddress(XElement element)
      {
         XElement cElement = element;
         string xPath = "";

         // We will iteratively go through the structure to the root element
         // The loop will stop at the root element (we don't include the root
         // element in the address !!)
         while (cElement.Parent != null)
         {
            bool forSeries = (cElement.Name.ToString().ToLower().Equals("series"));
            string secondValue = forSeries ? cElement.Attribute("Date").Value : cElement.Attribute("id").Value;

            xPath = String.Format("/{0}[@" + (forSeries ? "Date" : "id") + "='{1}']",
                     cElement.Name, secondValue)
                     + xPath;

            cElement = cElement.Parent;
         }

         return xPath;
      }
      #endregion

      #region Saving and Updating
      /// <summary>
      /// Test if there is an existing series with the same Date attribute value
      /// </summary>
      /// <param name="seriesDate">Date value to check against</param>
      /// <returns></returns>
      private bool seriesExists(string seriesDate)
      {
         return
            (
               ((IEnumerable<Object>)
                  this.root.XPathEvaluate("/" + SeriesInfo.Name + "[@Date='" + seriesDate + "']")
               ).Any()
            );
      }

      /// <summary>
      /// Deletes an element based on the xPath address
      /// </summary>
      /// <param name="xPath">XML xPath address to remove</param>
      public bool deleteItem(string xPath)
      {
         try
         {
            if (this.root == null) throw new Exception(EXCEPTION_ROOT_NULL);

            XElement itemElement = this.root.XPathSelectElement(xPath);

            itemElement.Remove();

            this.root.Save(this.filePath);

            return true;
         }
         catch (Exception ex) { this.formatError(ex); return false; }
      }

      public bool saveChanges(string xPath, SeriesInfo info) { return saveSeriesChanges(xPath, info); }
      public bool saveChanges(string xPath, ClassInfo info) { return saveClassChanges(xPath, info); }

      /// <summary>
      /// Saves changes to a Series based element on the xPath address
      /// </summary>
      /// <param name="xPath">XML xPath address to modify</param>
      /// <param name="seriesInfo">Series object that contains the new updated values</param>
      private bool saveSeriesChanges(string xPath, SeriesInfo seriesInfo)
      {
         try
         {
            if (this.root == null) throw new Exception(EXCEPTION_ROOT_NULL);

            XElement itemElement = this.root.XPathSelectElement(xPath);

            string oldSeriesDate = itemElement.Attribute("Date").Value;
            string newSeriesDate = seriesInfo.Date.ToShortDateString();

            bool dateChanged = !oldSeriesDate.Equals(newSeriesDate);

            // Only check the date value if it has been changed
            if (dateChanged)
            {
               // Test if there is an existing series with the same Date attribute.
               if (seriesExists(newSeriesDate))
                  throw new Exception(EXCEPTION_SERIES_UPDATE_EXISTS);
            }

            itemElement.SetAttributeValue("Date", newSeriesDate);
            itemElement.SetAttributeValue("CustomTitle", seriesInfo.CustomTitle.ToString().ToLower());
            itemElement.SetAttributeValue("Title", seriesInfo.Title);
            itemElement.SetAttributeValue("Visible", seriesInfo.Visible.ToString().ToLower());

            if (dateChanged)
            {
               DateTime dOld = DateTime.Parse(oldSeriesDate);
               DateTime dNew = DateTime.Parse(newSeriesDate);

               updateClassDates(itemElement, dNew.Subtract(dOld));
            }

            this.root.Save(this.filePath);

            return true;
         }
         catch (Exception ex) { this.formatError(ex); return false; }
      }

       /// <summary>
      /// Saves changes to a Class based element on the xPath address
      /// </summary>
      /// <param name="xPath">XML xPath address to modify</param>
      /// <param name="classInfo">Class object that contains the new updated values</param>
      /// <returns></returns>
      private bool saveClassChanges(string xPath, ClassInfo classInfo)
      {
         try
         {
            if (this.root == null) throw new Exception(EXCEPTION_ROOT_NULL);

            XElement itemElement = this.root.XPathSelectElement(xPath);

            // Update the class information
            itemElement.SetElementValue("start", classInfo.StartShortDate);
            itemElement.SetElementValue("mandatory", classInfo.MandatoryShortDate);
            itemElement.SetElementValue("mandatoryarray", classInfo.MandatoryDateArray);
            itemElement.SetElementValue("skip", classInfo.SkipShortDate);
            itemElement.SetElementValue("skiparray", classInfo.SkipDateArray);
            itemElement.SetElementValue("end", classInfo.EndShortDate);
            itemElement.SetElementValue("days", classInfo.Days);
            itemElement.SetElementValue("time", classInfo.Time);
            itemElement.SetElementValue("area", classInfo.Area);
            itemElement.SetElementValue("filled", classInfo.Filled.ToString().ToLower());
            itemElement.SetElementValue("cancelled", classInfo.Cancelled.ToString().ToLower());

            // Set the classInfo's ID so we can reference it back if needed
            classInfo.ID = int.Parse(itemElement.Attribute("id").Value);

            this.root.Save(this.filePath);

            return true;
         }
         catch (Exception ex) { this.formatError(ex); return false; }
      }

      /// <summary>
      /// Saves new Series item into the XML file.
      /// </summary>
      /// <param name="seriesInfo"></param>
      public bool saveNewItem(SeriesInfo seriesInfo)
      {
         try
         {
            if (this.root == null) throw new Exception(EXCEPTION_ROOT_NULL);

            string seriesDate = seriesInfo.Date.ToShortDateString();

            // Test if there is an existing series with the same Date attribute.
            if (seriesExists(seriesDate))
               throw new Exception(EXCEPTION_SERIES_NEW_EXISTS);

            //int maxId = 0;
            //if(this.root.HasElements)
               //maxId = this.root.Elements().Max(c => int.Parse(c.Attribute("id").Value));

            XElement newElement = new XElement(SeriesInfo.Name,
               //new XAttribute("id", (maxId + 1).ToString()),
               new XAttribute("Date", seriesDate),
               new XAttribute("Title", seriesInfo.Title),
               new XAttribute("CustomTitle", seriesInfo.CustomTitle.ToString().ToLower()),
               new XAttribute("Visible", seriesInfo.Visible.ToString().ToLower()));

            this.root.Add(newElement);

            this.root.Save(this.filePath);

            return true;
         }
         catch (Exception ex) { this.formatError(ex); return false; }
      }

      /// <summary>
      /// Saves new Class item into the XML file.
      /// </summary>
      /// <param name="xPath"></param>
      /// <param name="classInfo"></param>
      public bool saveNewItem(string xPath, ClassInfo classInfo)
      {
         try
         {
            if (this.root == null) throw new Exception(EXCEPTION_ROOT_NULL);

            XElement parentElement = this.root.XPathSelectElement(xPath);

            int maxId = 0;
            if(parentElement.HasElements)
               maxId = parentElement.Elements().Max(c => int.Parse(c.Attribute("id").Value));

            int newId = (maxId + 1);

            XElement newElement = new XElement(ClassInfo.Name,
               new XAttribute("id", newId.ToString()),
               new XElement("start", classInfo.StartDate.ToShortDateString()),
               new XElement("mandatory", classInfo.MandatoryShortDate),
               new XElement("mandatoryarray", classInfo.MandatoryDateArray),
               new XElement("skip", classInfo.SkipShortDate),
               new XElement("skiparray", classInfo.SkipDateArray),
               new XElement("end", classInfo.EndDate.ToShortDateString()),
               new XElement("days", classInfo.Days),
               new XElement("time", classInfo.Time),
               new XElement("area", classInfo.Area),
               new XElement("filled", classInfo.Filled.ToString().ToLower()),
               new XElement("cancelled", classInfo.Cancelled.ToString().ToLower())
            );

            parentElement.Add(newElement);

            // Set the classInfo's ID so we can reference it back if needed
            classInfo.ID = newId;

            this.root.Save(this.filePath);

            return true;
         }
         catch (Exception ex) { this.formatError(ex); return false; }
      }

      /// <summary>
      /// Updates the date ranges with dateDiff for a set of class nodes owned by parentRoot.
      /// </summary>
      /// <param name="parentRoot">The parent root of classes to update</param>
      /// <param name="dateDiff">The TimeSpan difference to update with</param>
      private void updateClassDates(XElement parentRoot, TimeSpan dateDiff)
      {
         IEnumerable<XElement> cList = parentRoot.XPathSelectElements(ClassInfo.Name);

         //return;

         foreach (XElement el in cList)
         {
            DateTime start = DateTime.Parse(el.Element("start").Value).Add(dateDiff);
            DateTime end = DateTime.Parse(el.Element("end").Value).Add(dateDiff);

            DateTime mandatory = DateTime.MinValue;
            string strMandatory = el.Element("mandatory").Value;
            if (!string.IsNullOrEmpty(strMandatory))
                mandatory = DateTime.Parse(strMandatory).Add(dateDiff);

            DateTime skip = DateTime.MinValue;
            string strSkip = el.Element("skip").Value;
            if (!string.IsNullOrEmpty(strSkip))
               skip = DateTime.Parse(strSkip).Add(dateDiff);

             el.SetElementValue("start", start.ToShortDateString());
            el.SetElementValue("end", end.ToShortDateString());
            el.SetElementValue("skip", (skip.Equals(DateTime.MinValue) ? "" : skip.ToShortDateString()));
            el.SetElementValue("mandatory", (mandatory.Equals(DateTime.MinValue) ? "" : mandatory.ToShortDateString()));
         }
      }
      #endregion

      #region Selecting Methods
      public IQueryable<SeriesInfo> SelectSeries()
      {
         return SelectSeries(false);
      }

      public IQueryable<SeriesInfo> SelectSeries(bool showVisibleOnly)
      {
         if (this.root == null) return null;

         IQueryable<SeriesInfo> series =
            from sElement in this.root.Descendants("Series").AsQueryable()
            let sDate = getXAttributeValue<DateTime>(sElement.Attribute("Date"))
            let visible = getXAttributeValue<Boolean>(sElement.Attribute("Visible"))
            where visible.Equals(true) || visible.Equals(showVisibleOnly)
            orderby sDate
            select new SeriesInfo
            {
               Date = sDate,
               CustomTitle = getXAttributeValue<Boolean>(sElement.Attribute("CustomTitle")),
               Title = sElement.Attribute("Title").Value,
               Visible = visible,
               XPathAddress = getXPathAddress(sElement)
            };

         return series;
      }

      public IQueryable<SeriesInfo> SelectSeriesVisible() { return SelectSeries(true); }

      public IQueryable<ClassInfo> SelectClasses(string parentXPath)
      {
         if (this.root == null || string.IsNullOrEmpty(parentXPath)) return null;

         IQueryable<ClassInfo> classes =
            from sElement in this.root.XPathSelectElements(parentXPath + "/Class").AsQueryable()
            let sDate = getXElementValue<DateTime>(sElement.Element("start"))
            let cancelled = getXElementValue<Boolean>(sElement.Element("cancelled"))
            orderby sDate
            select new ClassInfo
            {
               ID = int.Parse(sElement.Attribute("id").Value),
               StartDate = sDate,
               MandatoryDate = getXElementValue<DateTime>(sElement.Element("mandatory")),
               MandatoryDateArray = getXElementValue<String>(sElement.Element("mandatoryarray")),
               SkipDate = getXElementValue<DateTime>(sElement.Element("skip")),
               SkipDateArray = getXElementValue<String>(sElement.Element("skiparray")),
               EndDate = getXElementValue<DateTime>(sElement.Element("end")),
               Days = sElement.Element("days").Value,
               Time = sElement.Element("time").Value,
               Area = sElement.Element("area").Value,
               Filled = (cancelled ? false : getXElementValue<Boolean>(sElement.Element("filled"))),
               Cancelled = cancelled,
               XPathAddress = getXPathAddress(sElement)
            };

         return classes;
      }
      #endregion
   }
}