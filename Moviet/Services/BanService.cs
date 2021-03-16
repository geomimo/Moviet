using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.Services.Interfaces;
using System.Collections.Generic;

namespace Moviet.Services
{
    public class BanService : IBanService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postrepo;
        private readonly IMovieRepository _movierepo;
        private readonly IRatingRepository _ratingrepo;

        public BanService(UserManager<ApplicationUser> userManager, IPostRepository postrepo, IMovieRepository movierepo, IRatingRepository ratingrepo)
        {
            _userManager = userManager;
            _postrepo = postrepo;
            _movierepo = movierepo;
            _ratingrepo = ratingrepo;
        }

        public bool BanPost(int postId)
        {
            Post post = _postrepo.FindById(postId);
            Movie movie = _movierepo.FindById(post.Movie.MovieId);
            movie.PostRemoved = true;
            _movierepo.Update(movie);

            return _postrepo.Delete(post);
        }

        public bool BanUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            List<Rating> userRatings = _ratingrepo.FindAllByUserId(user.Id);
            foreach(var r in userRatings)
            {
                _ratingrepo.Delete(r);
            }

            return _userManager.DeleteAsync(user).Result.Succeeded;
        }
    }
}
