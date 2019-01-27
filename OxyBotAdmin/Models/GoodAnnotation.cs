using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class GoodAnnotation
    {

        public int AnnotationId { get; set; }

        [Required]
        public string DrugName { get; set; }

        [Required]
        public string Producer { get; set; }

        public string UsingWay { get; set; }

        public string ForWhatIsUse { get; set; }

        public string SpecialInstructions { get; set; }

        public string ContraIndicators { get; set; }

        public string SideEffects { get; set; }

        public bool IsImageExists { get; set; }

        public int TotalCountOfAnnotations { get; set; }
    }
}
