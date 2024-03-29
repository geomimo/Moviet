﻿using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Linq;

namespace Moviet.Mappings
{
    public class TotalRatingResolver : IValueResolver<Movie, MovieVM, float>
    {
        public float Resolve(Movie source, MovieVM destination, float destMember, ResolutionContext context)
        {
            return (float)Math.Round((double)source.Ratings.Average(r => r.Value), 1);

        }
    }
}
