using Telegram.Bot.Types;
using TelegramProviders.Domain.Models;

namespace TelegramProviders
{
    public interface ITelegramUpdateReader
    {
        UserRequest ReadUpdate(Update update);
    }
}