using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramProviders.Domain;
using TelegramProviders.Domain.Models;

namespace TelegramProviders
{
    public class TelegramProvider : IMessengerProvider
    {
        private readonly ITelegramUpdateReader _converter;
        private readonly ITelegramBotClient _telegramClient;

        public TelegramProvider(ITelegramBotClient telegramClient,
            ITelegramUpdateReader converter)
        {
            _converter = converter;
            _telegramClient = telegramClient ?? throw new ArgumentNullException(nameof(telegramClient));
        }

        public async Task<UserRequestBatch> GetUserRequests(int offset)
        {
            var updates = await _telegramClient.GetUpdatesAsync(offset);
            if (!updates.Any())
            {
                return null;
            }
            var nextOffset = updates.Max(u => u.Id) + 1;
            var userRequests = updates.Select(_converter.ReadUpdate).ToList();
            var result = new UserRequestBatch(userRequests, nextOffset);
            return result;
        }

        public async Task SendMessage(string userId, string message)
        {
            await _telegramClient.SendTextMessageAsync(userId, message);
        }
    }
}
