using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    interface IRecommendationService
    {
        public List<Post> GetRecommendation(int n, string userId);
        public void Train();
    }
}
