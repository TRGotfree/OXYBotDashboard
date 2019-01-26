using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OxyBotAdmin.Models
{
    public class GoodAnnotation
    {
        public int AnnotationId { get; set; }

        public string DrugName { get; set; }

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
