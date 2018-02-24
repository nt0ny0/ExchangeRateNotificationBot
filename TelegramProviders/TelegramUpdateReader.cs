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
            UserRequest result;
            switch (update.Type)
            {
                case UpdateType.MessageUpdate:
                {
                    var message = update.Message;
                    if (message == null)
                    {
                        return null;
                    }
                    var requestType = _requestParser.Parse(message.Text);
                    result = new UserRequest(message.From.Id.ToString(), requestType);
                    break;
                }
                default:
                {
                    result = new UserRequest(null, UserRequestType.Unknown);
                    break;
                }
            }
            return result;
        }
    }
}
