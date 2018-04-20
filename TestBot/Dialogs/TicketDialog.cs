using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Threading;
using OpenQA.Selenium.Remote;

namespace TestBot.Dialogs
{
    [Serializable]
    public class TicketDialog : IDialog<string>
    {
        
        private string name;
        private int attempts = 3;
        public string response;

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
            
            String ticketID;
            TicketHandler ticketH;


            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            
                
                ticketID = activity.Text;

                if (ticketID.ToLower().Contains("i") || ticketID.ToLower().Contains("s") && ticketID.Contains("_"))
                {
                ticketH = new TicketHandler(ticketID);
                if (ticketID.ToLower().StartsWith("s"))
                {
                    ticketH.IsIncident = false;
                }
                else
                {
                    ticketH.IsIncident = true;
                }
                    
                }
                else
                {
                    ticketID = "";
                    await context.PostAsync("Input invalid please enter ticket number again : (I123456_123456)");

                    return;
                }
                //Verify string format

            
            

            var options = new InternetExplorerOptions();
            options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;

            // Search for ticket in EV
            IWebDriver driver = new InternetExplorerDriver(options);
            ticketH.Driver = driver;


            //Calls method to get all the ticket information
            if (ticketH.IsIncident)
            {
                response = ticketH.getTicketInfo(ticketID);
                await context.PostAsync(response);
            }
            else
            {
                response = ticketH.getServiceRequest(ticketID);
                await context.PostAsync(response);
            }

            if (response != "The ticket could not be found please try again. Trials")
            {
                TicketHandler.sendEmailToTech(ticketH.Name, "ALERT URGENT", "User for " + ticketH.ticketID + " is checking their ticket respond to them now.");
            }

            context.Done(ticketID);
        }
    }
}