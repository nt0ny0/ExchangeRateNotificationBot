using System;
using TelegramProviders.Domain;

namespace Bot
{
    public class ExchangeRateBotHelpCommandTextBuilder : IHelpCommandTextBuilder
    {
        private static readonly string _helpText = $@"
<b>Список команд</b>:
{UserCommands.UsdToRubGetRate} - текущий курс доллара к рублю
{UserCommands.EuroToRubGetRate} - текущий курс евро к рублю
{UserCommands.EuroToUsdGetRate} - текущий курс доллара к евро";

        public string BuildHelpCommandText()
        {
            return _helpText;
        }

        public string BuildErrorMessage(Exception exception)
        {
            return "Сервис временно недоступен";
        }
    }
}
