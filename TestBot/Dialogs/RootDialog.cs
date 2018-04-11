using System;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace TestBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        

        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            var actions = new List<CardAction>();

            actions.Add(new CardAction
            {
                Title = $"Add Printer",
                Text = $"Printer Action",
                Value = $"printer"
            });
            actions.Add(new CardAction
            {
                Title = $"Clear Citrix Session",
                Text = $"Citrix Action",
                Value = $"citrix"
            });
            actions.Add(new CardAction
            {
                Title = $"Check on incident",
                Text = $"incident",
                Value = $"incident"
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

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            //await context.PostAsync("Would you like to check on an ticket or service request?");
            var activity = await argument as Activity;
            


            string str = activity.Text;
            //TicketHandler.sendEmailToTech("jeremy farmer", "Hello subject", "Hello body");
            //await this.WelcomeDialogAsync(context, argument);
            if (str.ToLower().Contains("ticket") || str.ToLower().Contains("incident"))
            {

                context.Call(new TicketDialog(), this.ResumeAfterTicketDialog);
            }
            else if (str.ToLower().Contains("printer"))
            {
                context.Call(new PrinterDialog(), this.ResumeAfterTicketDialog);
            }
            else if (str.ToLower().Contains("citrix"))
            {
                context.Call(new CitrixSessionDialog(), this.ResumeAfterTicketDialog);
            }
            else if (str.ToLower().Contains("password"))
            {
                context.Call(new PasswordChangeDialog(), this.ResumeAfterTicketDialog);
            }
            else
            {
                
            }

        }


        private async Task WelcomeDialogAsync(IDialogContext context, IAwaitable<IMessageActivity> message)
        {
            var activity = await message as Activity;

           

            string str = activity.Text;
            await context.PostAsync("Your message : " + str);

            if (str.ToLower().Contains("ticket") || str.ToLower().Contains("incident"))
            {
                context.Call(new TicketDialog(), this.ResumeAfterTicketDialog);
            }else
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
       


        private async Task ResumeAfterTicketDialog(IDialogContext context, IAwaitable<string> result)
        {
            var resultFromNewOrder = await result;
            await context.PostAsync("Send another message to start over.");

            context.Wait(this.MessageReceivedAsync);
        }


        
    }
}