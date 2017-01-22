using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;

namespace DirectLineGettingStarted
{
    public class MyDirectLineIntegration
    {
        private readonly DirectLineClient _directLineClient;
        private Conversation _conversation;
        private string _watermark;

        private const string From = "Kristian";

        public MyDirectLineIntegration()
        {
            const string secret = "YOUR_SECRET";
            _directLineClient = new DirectLineClient(secret);
        }

        public async Task StartConversation()
        {
            _conversation = await _directLineClient.Conversations
                .StartConversationAsync().ConfigureAwait(false);
        }

        public async Task SendMessage(string message)
        {
            var fromProperty = new ChannelAccount(From);
            var activity = new Activity(text: message, fromProperty: fromProperty);
            await _directLineClient.Conversations
                .PostActivityAsync(_conversation.ConversationId, activity).ConfigureAwait(false);
        }

        public async Task<IList<Activity>> GetMessages()
        {
            var response = await _directLineClient.Conversations
                .GetActivitiesAsync(_conversation.ConversationId, _watermark).ConfigureAwait(false);
            _watermark = response.Watermark;
            return response.Activities;
        }
    }
}
