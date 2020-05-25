using Moviet.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services
{
    public class YoutubeService : IYoutubeService
    {
        public string ConvertUrl(string url)
        {
            string id = url.Split("v=").Last();
            return "https://www.youtube.com/embed/" + id;
        }
    }
}
