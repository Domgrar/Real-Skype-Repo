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
            await context.PostAsync("");

            context.Wait(this.MessageReceivedAsync);


        }

        //Probably needs to grab from a text file that is easily editable

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            




            context.Done("finished");
        }

    }
}