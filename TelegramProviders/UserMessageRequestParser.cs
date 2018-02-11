using System;
using TelegramProviders.Domain;
using TelegramProviders.Domain.Models;

namespace TelegramProviders
{
    public interface IUserMessageRequestParser
    {
        UserRequestType Parse(string updateText);
    }

    public class UserMessageRequestParser : IUserMessageRequestParser
    {
        public UserRequestType Parse(string updateText)
        {
            if (string.IsNullOrEmpty(updateText))
            {
                return UserRequestType.Unknown;
            }
            if (updateText.Equals("/start", StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetComandList;
            }
            if (updateText.Equals("/rate", StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetExchangeRate;
            }
            return UserRequestType.Unknown;
        }
    }
}
