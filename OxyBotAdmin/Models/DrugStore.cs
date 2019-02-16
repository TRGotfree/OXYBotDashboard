using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class DrugStore
    {
        public uint Id { get; set; }

        [Range(1, 10000, ErrorMessage = "Укажите Id аптеки из \"Аналитики\"")]
        public uint DrugStoreId { get; set; }

        [Required(AllowEmptyStrings = false)]
        //[RegularExpression(@"^Аптека №\d+\s\W+|^Аптека№\d+\W+|^Аптека №\s\d+\W+", ErrorMessage = "Название аптеки должно содержать слово Аптека и № аптеки")]
        public string DrugStoreName { get; set; }

        public string Address { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Phone { get; set; }

        public string WorkTime { get; set; }

        public string Orientir { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string District { get; set; }

        [Required]
        //[RegularExpression(@"^Аптека №\d+$|^Аптека№\d+$|Аптека №\s\d+$")]
        public string ShortName { get; set; }

        public int DrugStoreTotalCount { get; set; }
    }
}
