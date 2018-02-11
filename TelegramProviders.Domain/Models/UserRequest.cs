namespace TelegramProviders.Domain.Models
{
    public class UserRequest
    {
        public string UserId { get; }
        public UserRequestType UserRequestType { get; }

        public UserRequest(string userId, UserRequestType userRequestType)
        {
            UserId = userId;
            UserRequestType = userRequestType;
        }
    }
}
