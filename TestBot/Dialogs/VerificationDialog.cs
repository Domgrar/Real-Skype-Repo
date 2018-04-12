using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System.Threading;
using System.Collections.Generic;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace TestBot.Dialogs
{
    [Serializable]
    public class VerificationDialog : IDialog<string>
    {

        static Random rnd = new Random();
        int verification = 222222;           // rnd.Next(111111, 999999);



        public VerificationDialog()
        {
            
        
        }
        public async Task StartAsync(IDialogContext context)
        {

            //Initial dialog - returns the response to the MessageReceivedAsync method below
            await context.PostAsync("A verification number has been texted to you please type it in");

            // Call send message method
            //sendSMS(verification);

            context.Wait(this.MessageReceivedAsync);
            

        }

        //check format of number for validation

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {

            var activity = await result as Activity;
            
            int userInput = Convert.ToInt32(activity.Text.Trim());
            if(userInput == verification)
            {
                //Change password
                
                context.Call(new PasswordChangeDialog(), this.ResumeAfterTicketDialog);
            }
            else
            {
                await context.PostAsync("Verification codes didn't match try again");
            }








            
        }
        private async Task ResumeAfterTicketDialog(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromNewOrder = await result;
            await context.PostAsync("Send another message to start over.");

            context.Wait(this.MessageReceivedAsync);
        }

        public static void sendSMS(int numberToVerif)
        {
            var accountSid = "ACb3b26579dabfec81cac5f65493bf4e27";

            var authToken = "e1edb771c6c20119134b4ab4c0d99350";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: new PhoneNumber("+18288086822"),
                from: new PhoneNumber("+19803658236"),
                body: numberToVerif.ToString());

            
        }
    }
}