using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramProviders.Domain.Models;

namespace TelegramProviders.Domain
{
    public interface IMessengerProvider
    {
        Task<UserRequestBatch> GetUserRequests(int offset);
        Task SendMessage(string userId, string message);
    }
}
