using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Models
{
    public class EvaluationResultsVM
    {
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double RMS { get; set; }
        [Display(Name = "Loss Function")]
        public double LossFunction { get; set; }
        public double R2 { get; set; }
        [Display(Name = "Date Trained")]
        public DateTime DateTrained { get; set; }
    }
}
