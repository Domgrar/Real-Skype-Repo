using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace TestBot.Dialogs
{
    [Serializable]
    public class CheckOuttageDialog : IDialog<string>
    {




        public CheckOuttageDialog()
        {

        }

        public async Task StartAsync(IDialogContext context)
        {
            //Button to select "check for outtages"
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var actions = new List<CardAction>();

            actions.Add(new CardAction
            {
                Title = $"Check Outtage",
                Text = $"Printer Action",
                Value = $"Check for any Outtages"
            });

            reply.Attachments.Add(
                new HeroCard
                {
                    Title = "Select what you would like to do:",
                    Buttons = actions
                }.ToAttachment()
            );


            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);

            


        }

        //Probably needs to grab from a text file that is easily editable

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // pull from file somewhere




            context.Done("finished");
        }

    }
}