using System.Threading.Tasks;

namespace TelegramProviders.Domain
{
    public interface IMessengerBot
    {
        Task Start();
        void Stop();
    }
}