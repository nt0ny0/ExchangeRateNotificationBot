using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramProviders.Domain.Models;

namespace TelegramProviders
{
    public class TelegramUpdateReader : ITelegramUpdateReader
    {
        private readonly IUserMessageRequestParser _requestParser;
        public TelegramUpdateReader(IUserMessageRequestParser requestParser)
        {
            _requestParser = requestParser ?? throw new ArgumentNullException(nameof(requestParser));
        }
        public UserRequest ReadUpdate(Update update)
        {
            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }
            if (update.Type == UpdateType.MessageUpdate)
            {
                var message = update.Message;
                if (message == null)
                {
                    return null;
                }
                var requestType = _requestParser.Parse(message.Text);
                return new UserRequest(message.From.Id.ToString(), requestType);
            }
            return new UserRequest(null, UserRequestType.Unknown);
        }
    }
}
