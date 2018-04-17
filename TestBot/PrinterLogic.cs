using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestBot
{
    public class PrinterLogic
    {
        public IWebDriver Driver { get; set; }
        public String Name { get; set; }
        public string ticketID { get; set; }

        public void navigateToPrinters(string officeLocation)
        {
            
            this.Driver.Navigate().GoToUrl("http://dcprtmon02.corp.local/"); //http://dcprtmon02.corp.local/144-548-301/
        }

    }
}