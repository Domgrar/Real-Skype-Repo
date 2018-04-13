using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace TestBot.Dialogs
{
    [Serializable]
    public class TicketDialog : IDialog<string>
    {
        
        private string name;
        private int attempts = 3;

        public TicketDialog()
        {
            
        }

        public async Task StartAsync(IDialogContext context)
        {
            //Initial dialog - returns the response to the MessageReceivedAsync method below
            //await context.PostAsync("Please copy and paste in your ticket number:");
            await context.PostAsync(this.name + "Please copy and paste in your incident ID number");

            context.Wait(this.MessageReceivedAsync);

            
        }

        //check format of number for validation

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            
            

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;
            String ticketID = activity.Text;
            TicketHandler ticketH = new TicketHandler(ticketID);

            // Search for ticket in EV
            IWebDriver driver = new ChromeDriver();
            ticketH.Driver = driver;
            
            //Calls method to get all the ticket information
            string response = ticketH.getTicketInfo(ticketID);
            await context.PostAsync(response);

            if (response != "The ticket could not be found please try again.")
            {
                TicketHandler.sendEmailToTech(ticketH.Name, "ALERT URGENT", "User for " + ticketH.ticketID + " is checking their ticket respond to them now.");
            }

            context.Done(ticketID);
        }
    }
}