using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Net.Http;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System.Threading;

namespace TestBot.Dialogs
{
    [Serializable]
    public class PrinterDialog : IDialog<string>
    {

            

            public PrinterDialog()
            {

            }

            public async Task StartAsync(IDialogContext context)
            {
            //Initial dialog - returns the response to the MessageReceivedAsync method below
            //await context.PostAsync("Please copy and paste in your ticket number:");
            await context.PostAsync("What printer are you looking to download?");

            context.Wait(this.MessageReceivedAsync);


            }

            //check format of number for validation

            private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
            {
                var activity = await result as Activity;

            await context.PostAsync("Click on the link below to download the specified printer " +
               " it will give you a pop up you have to click yes on " +
               "http://dcprtmon02.corp.local/144-548-301/");



            context.Done("finished");
            }
        }
    }





