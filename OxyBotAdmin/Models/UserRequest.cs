using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class UserRequest
    {
        public long RequestId { get; set; }
        public string RequestText { get; set; }
        public long ChatId { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
