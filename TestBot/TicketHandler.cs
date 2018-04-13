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
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetFocusedWindow();

        [DllImport("users32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        

        public IWebDriver Driver { get; set; }
        public String Name { get; set; }
        public string ticketID { get; set; }

        public TicketHandler(string ticket)
        {
            this.ticketID = ticket;
        }


        public string getTicketInfo(string ticketNumber)
        {
            string managerName;
            string response;
            string progressText;
            
            this.Driver.Navigate().GoToUrl("https://dhgllp.easyvista.com/");

            

            


            

            Thread.Sleep(4000);
            
            IWebElement incidentSearch = this.Driver.FindElement(By.XPath("//input[@name='GlobalSearchText']"));
            incidentSearch.SendKeys(ticketNumber);
            incidentSearch.SendKeys(OpenQA.Selenium.Keys.Enter);
            Thread.Sleep(7000);

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