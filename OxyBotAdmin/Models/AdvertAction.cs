using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class AdvertAction
    {
        public uint ActionId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(2000, ErrorMessage = "AdvertisingTextIsTooLong")]
        public string AdvertisingText { get; set; }

        [MaxLength(30, ErrorMessage = "AdvertShortTextLengthIsTooLong")]
        public string AdvertisingTextShort { get; private set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "AdvertNameNotSet")]
        [MaxLength(50, ErrorMessage = "NameOfActionTooLong")]
        public string NameOfAction { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CommandTextNotSet")]
        [MaxLength(30, ErrorMessage = "CommandTextIsTooLong")]
        public string CommandText { get; set; }
       
        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        [Required]
        public bool State { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FormattedDateBegin { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FormattedDateEnd { get; set; }

        public int TotalCountOfAdvertActions { get; set; }

        public void SetFormattedDateBegin(DateTime dateBegin)
        {
            this.FormattedDateBegin = dateBegin.ToString("yyyy-MM-dd");
        }

        public void SetFormattedDateEnd(DateTime dateEnd)
        {
            this.FormattedDateEnd = dateEnd.ToString("yyyy-MM-dd");
        }

        public void SetAdvertisingTextShort(string advertFullText)
        {
            if (string.IsNullOrEmpty(advertFullText) || string.IsNullOrWhiteSpace(advertFullText))
                throw new ArgumentNullException(nameof(advertFullText));

            if (advertFullText.Length > 30)
                this.AdvertisingTextShort = advertFullText.Substring(0, 30);          
            else
                this.AdvertisingTextShort = advertFullText;
        }
    }
}
