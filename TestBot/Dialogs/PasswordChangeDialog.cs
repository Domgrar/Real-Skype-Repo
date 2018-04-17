using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System.Threading;
using System.Collections.Generic;

namespace TestBot.Dialogs
{
    [Serializable]
    public class PasswordChangeDialog : IDialog<string>
    {
        



        public PasswordChangeDialog()
            {

            }

            public async Task StartAsync(IDialogContext context)
            {
                //Initial dialog - returns the response to the MessageReceivedAsync method below
                //await context.PostAsync("Please copy and paste in your ticket number:");
                await context.PostAsync("Can you connect to our VPN? (ciscoAnyConnect)");
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var actions = new List<CardAction>();

            actions.Add(new CardAction
            {
                Title = $"Yes",
                Text = $"Yes",
                Value = $"yes"
            });
            actions.Add(new CardAction
            {
                Title = $"No",
                Text = $"No",
                Value = $"no"
            });
            
            reply.Attachments.Add(
                new HeroCard
                {
                    Title = "Select your answer",
                    Buttons = actions
                }.ToAttachment()
            );


            await context.PostAsync(reply);
            

            context.Wait(this.MessageReceivedAsync);


            }

            //check format of number for validation

            private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
            {
            var activity = await result as Activity;


            if (activity.Text.Contains("yes"))
            {
      
                await context.PostAsync("You can use control alt delete to change your password");
            }
            else
            {
                await context.PostAsync("Go to this site - C:\\Users\\jf6856\\Desktop\\File Store\\index.html \n and click the download button \n " +
                    "There is documentation there if you need assistance \n" +
                    "Please select the Allow Blocked Contect in the bottom of the page!");
            }



                
                

                

                context.Done("Finished");
            }
       
    }
   
}