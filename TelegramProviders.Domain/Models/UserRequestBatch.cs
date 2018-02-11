using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramProviders.Domain.Models
{
    public class UserRequestBatch
    {
        public List<UserRequest> Requests { get; }
        public int Offset { get; }

        public UserRequestBatch(List<UserRequest> requests, int offset)
        {
            Requests = requests;
            Offset = offset;
        }
    }
}
