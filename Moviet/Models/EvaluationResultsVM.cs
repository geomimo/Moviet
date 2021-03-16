using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class EvaluationResultsVM
    {
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public double L1 { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public double L2 { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public double RMS { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        [Display(Name = "Loss Function")]
        public double LossFunction { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public double R2 { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Trained")]
        public DateTime DateTrained { get; set; }
    }
}
