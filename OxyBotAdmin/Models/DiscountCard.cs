using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class DiscountCard
    {
        [Required]
        public ulong ChatId { get; set; }

        [Required]
        public uint CardId { get; set; }

        [Required]
        public string UserFIO { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Email { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public bool IsUserWantsToGetUpdates { get; set; }

        [Required]
        public bool IsRegistered { get; set; }

        public string DateTimeEntered { get; set; }

        public int TotalCountOfCardsData { get; set; }
    }
}
