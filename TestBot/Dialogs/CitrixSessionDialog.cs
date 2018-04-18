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
    public class CitrixSessionDialog : IDialog<string>
    {
        



            public CitrixSessionDialog()
            {

            }

            public async Task StartAsync(IDialogContext context)
            {
                //Initial dialog - returns the response to the MessageReceivedAsync method below
                //await context.PostAsync("Please copy and paste in your ticket number:");
                await context.PostAsync("Please enter your 2 letter 4 digit user ID (i.e aa1234)");

                context.Wait(this.MessageReceivedAsync);


            }

            //check format of number for validation

            private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
            {
            var activity = await result as Activity;

            //While loop to wait while another SessionMangager is trying to kill session
            while(Process.GetProcessesByName("SessionManager.exe").Length > 0)
            {
                Thread.Sleep(4000);
            }

            ProcessStartInfo PSI = new ProcessStartInfo("SessionManager.exe"); //         //"param1 jf6856"
            PSI.Arguments = "param1 " + activity.Text.Trim();
            PSI.WorkingDirectory = "C:\\Users\\jf6856\\Desktop\\";
            PSI.UseShellExecute = true;

            await context.PostAsync("Moving to clear the citrix session wait one moment");


            Process Proc = Process.Start(PSI);
            Proc.WaitForExit();


            await context.PostAsync("Session should be cleared. Contact the service desk if issues persist");
            
                


            context.Done("finished");
            }
        
    }
}