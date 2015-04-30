PS-MAPP Online Registration
========

A Visual Studio 2010 project.

This application stores its date in an XML file under ~/data/schedule.xml.
There is no sensitive data saved in this file, simply class information and dates.
This application is originally hosted on the HDYS/Community College Foundation websites: http://hdys2.communitycollege.org/

The following is an example of how the class table appears once the XML file is processed:

| STARTS | MANDATORY CLASS DATES | SKIP  | ENDS  | DAYS  | HOURS | CITY     |        |
| ------ | --------------------- | ----- | ----- | ----- | ----- | -------- | ------ |
| 01/01  | 01/01, 02/01, 03/01   | 04/01 | 09/01 | M & W |  9-4  | South LA | FILLED |

When submitted, the online application will generate an email and send it to the email address defined in the web.config file appSettings key: TCCF_ContactEmail.

The email will be sent in HTML format. To test the email, include the xmlEmailTest.aspx file in your project and open that in a new browser window. The email should be generated off of the same XML file stored in ~/data/schedule.xml.
