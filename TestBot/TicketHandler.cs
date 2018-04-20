using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Text;
using System.Diagnostics;

namespace TestBot
{
    public class TicketHandler
    {
        
        public IWebDriver Driver { get; set; }
        public String Name { get; set; }
        public string ticketID { get; set; }
        public bool IsIncident { get; set; }

        public TicketHandler(string ticket)
        {
            this.ticketID = ticket;
        }
        public TicketHandler(string ticket, bool isIncident)
        {
            this.ticketID = ticket;
            this.IsIncident = isIncident;
        }


        public string getTicketInfo(string ticketNumber)
        {
            string managerName;
            string response;
            string progressText;
            IWebElement dropDownMenu;
            
            this.Driver.Navigate().GoToUrl("https://dhgllp.easyvista.com/");

            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //Check if searching for a service request or incident nubmer
            dropDownMenu = this.Driver.FindElement(By.XPath("//input[@id='GlobalCurrentQuerycombo-ui']"));

            dropDownMenu.SendKeys(Keys.Control + "a");
            dropDownMenu.SendKeys("Incidents");
            dropDownMenu.SendKeys(Keys.ArrowDown);
            dropDownMenu.SendKeys(Keys.Enter);

            //Thread.Sleep(4000);
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IWebElement incidentSearch = this.Driver.FindElement(By.XPath("//input[@name='GlobalSearchText']"));
            
            incidentSearch.SendKeys(ticketNumber);
            incidentSearch.SendKeys(OpenQA.Selenium.Keys.Enter);


            //Thread.Sleep(7000);
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            try
            {
                IWebElement managerElement = this.Driver.FindElement(By.XPath("//div[contains(@class, 'awesomeMainDiv')]"));
                managerName = managerElement.Text;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Manager not found");
                managerName = "N/A";
            }

            try
            {
                IWebElement progressElement = this.Driver.FindElement(By.XPath("//input[contains(@id, 'SD_STATUS.STATUS_EN')] [contains(@class, 'form_input_ro')]"));
                progressText = progressElement.GetAttribute("value");
            }catch(Exception ex)
            {
                Console.WriteLine("Progress not found");
                progressText = "N/A";
            }
            

            if (managerName != "N/A")
            {
                managerName = managerName.Split(',')[1] + " " + managerName.Split(',')[0];

                response = "The ticket is with technican : " + managerName + " and the status is : " + progressText + ", they will be in touch as soon as possible.";
            }else
            {

                response = "The ticket is not currently with a technician (adding more here later)";

            }

            this.Name = managerName;
            this.ticketID = ticketID;

            this.Driver.Close();
            return response;
        }

       public string getServiceRequest( string ticketNumber)
        {
            string response;
            IWebElement managerName;
            IWebElement progressStatus;
            IWebElement dropDownMenu;
            string targetDate;

            this.Driver.Navigate().GoToUrl("https://dhgllp.easyvista.com/");
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //Change to search for service request
            dropDownMenu = this.Driver.FindElement(By.XPath("//input[@id='GlobalCurrentQuerycombo-ui']"));

            dropDownMenu.SendKeys(Keys.Control + "a");
            dropDownMenu.SendKeys("Service Requests");
            dropDownMenu.SendKeys(Keys.ArrowDown); dropDownMenu.SendKeys(Keys.ArrowDown);
            dropDownMenu.SendKeys(Keys.Enter);


            //Send ticket Number
            IWebElement incidentSearch = this.Driver.FindElement(By.XPath("//input[@name='GlobalSearchText']"));

            incidentSearch.SendKeys(ticketNumber);
            incidentSearch.SendKeys(OpenQA.Selenium.Keys.Enter);


            //Scrape for information          
            targetDate = this.Driver.FindElement(By.XPath("//input[@name='SD_REQUEST.MAX_RESOLUTION_DATE_UT")).Text;

            response = "The request is expected to be there/setup on " + targetDate + " Please contact the service desk if it is past this target date. ";

            return response;
        }


        public static void sendEmailToTech(String techName, String subject, String body)
        {
            string emailName = techName.Trim().Replace(' ', '.') + "@dhgllp.com";

            Outlook.Application app = new Outlook.Application();
            Outlook.MailItem mailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
            mailItem.Subject = subject;
            mailItem.To = emailName;
            mailItem.Body = body;
            
            mailItem.Importance = Outlook.OlImportance.olImportanceHigh;
            mailItem.Display(false);

            //mailItem.Send();     //Add this in when ready to send
        }

    }
}