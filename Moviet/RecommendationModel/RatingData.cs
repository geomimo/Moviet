﻿using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moviet.RecommendationModel
{
    class RatingData
    {
        public float RaterId { get; set; }
        public float MovieId { get; set; }
        public float Value { get; set; }
        
    }
}
