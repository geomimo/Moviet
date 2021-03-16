using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Data
{
    public class EvaluationResults
    {
        [Key]
        public int Id { get; set; }
        public double L1 { get; set; }
        public double L2 { get; set; }
        public double RMS { get; set; }
        public double LossFunction { get; set; }
        public double R2 { get; set; }
        public DateTime DateTrained { get; set; }
    }
}
