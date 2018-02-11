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
            if (updateText.Equals(UserCommands.GetHelp, StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetComandList;
            }
            if (updateText.Equals(UserCommands.UsdToRubGetRate, StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetUsdToRubExchangeRate;
            }
            if (updateText.Equals(UserCommands.EuroToRubGetRate, StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetEuroToRubExchangeRate;
            }
            if (updateText.Equals(UserCommands.EuroToUsdGetRate, StringComparison.OrdinalIgnoreCase))
            {
                return UserRequestType.GetEuroToUsdExchangeRate;
            }
            return UserRequestType.Unknown;
        }
    }
}
