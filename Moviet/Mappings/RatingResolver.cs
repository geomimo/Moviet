using AutoMapper;
using Moviet.Data;
using Moviet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Mappings
{
    public class RatingResolver : IValueResolver<CreateMovieVM, Movie, List<Rating>>
    {
        public List<Rating> Resolve(CreateMovieVM source, Movie destination, List<Rating> destMember, ResolutionContext context)
        {
            return new List<Rating> { new Rating { DateRated = DateTime.Now, Value = source.Rating } };

        }
    }

}
