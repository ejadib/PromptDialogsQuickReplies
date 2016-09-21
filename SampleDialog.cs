namespace PromptChoiceQuickReplies
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class SampleDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            await context.PostAsync("Hi! I'm sample bot that will show you how to use Facebook's Quick Replies with a PromptChoice dialog");

            var promptOptions = new PromptOptions<string>(
                "Please select your age range:", 
                options: new[] { "20-35", "36-46", "47-57", "58-65", "65+" }, 
                promptStyler: new FacebookQuickRepliesPromptStyler());

            PromptDialog.Choice(context, this.ResumeAfterSelection, promptOptions);
        }

        private async Task ResumeAfterSelection(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var ageRange = await result;

                var message = $"Age range selected: {ageRange}";

                await context.PostAsync(message);
            }
            catch (TooManyAttemptsException)
            {
            }

            context.Wait(this.MessageReceivedAsync);
        }
    }
}