namespace PromptChoiceQuickReplies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using QuickReplies.Models;

    [Serializable]
    public class FacebookQuickRepliesPromptStyler : PromptStyler
    {
        public override void Apply<T>(ref IMessageActivity message, string prompt, IList<T> options)
        {
            if (message.ChannelId.Equals("facebook", StringComparison.InvariantCultureIgnoreCase) && this.PromptStyle == PromptStyle.Auto && options != null && options.Any())
            {
                var channelData = new FacebookChannelData();

                var quickReplies = new List<FacebookQuickReply>();

                foreach (var option in options)
                {
                    var quickReply = option as FacebookQuickReply;

                    if (quickReply == null)
                    {
                        quickReply = new FacebookTextQuickReply(option.ToString(), option.ToString());
                    }

                    quickReplies.Add(quickReply);
                }

                channelData.QuickReplies = quickReplies.ToArray();

                message.Text = prompt;
                message.ChannelData = channelData;
            }
            else
            {
                base.Apply<T>(ref message, prompt, options);
            }
        }
    }
}