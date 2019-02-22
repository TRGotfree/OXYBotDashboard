using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class TelegramUser
    {
        public int Id { get; set; }

        public long ChatId { get; set; }

        public string NickName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FirstAndLastName { get; set; }
    }
}
