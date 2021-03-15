using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using MovietML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public class RecommendationService : IRecommendationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRatingRepository _ratingsrepo;
        private readonly IPostRepository _postrepo;

        public List<Post> GetRecommendation(int n, string userId)
        {
            // Find all posts that the users has rated
            var ratedPosts = _postrepo.FindAllRatedByUserId(userId);

            // Find top 3 genres of the movies.
            var topGenres = GetTopGenres(ratedPosts);

            // Get all posts that have movie with genre in top 3 and have not yet watched.

            var postNotWatched = _postrepo.FindAll().Where(p => p.Movie.Genres.Any(g => topGenres.Contains(g.GenreId))) // top genres
                                                 .Where(p => !ratedPosts.Select(p => p.PostId).ToList().Contains(p.PostId))
                                                 .ToList(); // not watched

            // Get the movieId list of postNotSeen
            var topMovieIdsNotWatched = postNotWatched.Select(p => p.Movie.MovieId).ToList();

            // Check if user is new -> no trained model for him
            var userIsNew = _userManager.FindByIdAsync(userId).Result.IsNew;
            List<Post> result;
            if (userIsNew)
            {
                // Just pick random non watched movies
                result = GetRandomPosts(n, postNotWatched, topMovieIdsNotWatched);

            }
            else
            {
                // For each movie not watched, predict its rating
                var movieRatingDict = PredictRatings(topMovieIdsNotWatched, userId);

                // Get top 40%
                var topMovieIds = movieRatingDict.OrderBy(d => d.Value)
                                                 .Take((int)Math.Round(movieRatingDict.Count * 0.4, 0))
                                                 .Select(d => d.Key)
                                                 .ToList();

                result = GetRandomPosts(n, postNotWatched, topMovieIds);
            }

            return result;

        }

        public void Train()
        {
            throw new NotImplementedException();
        }

        private List<int> GetTopGenres(List<Post> ratedPosts)
        {
            var genreCounts = new Dictionary<int, int>();
            foreach (var post in ratedPosts)
            {
                foreach (var genre in post.Movie.Genres)
                {
                    if (!genreCounts.ContainsKey(genre.GenreId))
                    {
                        genreCounts[genre.GenreId] = 0;
                    }
                    genreCounts[genre.GenreId] += 1;
                }
            }

            int take = genreCounts.Count() >= 3 ? 3 : genreCounts.Count();
            return genreCounts.OrderBy(d => d.Value).Take(take).Select(d => d.Key).ToList();
        }

        private Dictionary<int, float> PredictRatings(List<int> movieIds, string userId)
        {
            var movieRatingDict = new Dictionary<int, float>();
            foreach (var movieId in movieIds)
            {
                var input = new ModelInput
                {
                    MovieId = movieId,
                    RaterId = float.Parse(userId)
                };

                movieRatingDict[movieId] = ConsumeModel.Predict(input).Score;
            }

            return movieRatingDict;
        }

        private List<Post> GetRandomPosts(int n, List<Post> posts, List<int> movieIds)
        {
            var random = new Random();
            var result = new List<Post>();
            n = Math.Min(n, movieIds.Count());

            var topPosts = posts.Where(p => movieIds.Contains(p.Movie.MovieId)).ToList();
            for (int i = 0; i < n; i++)
            {
                var index = random.Next(topPosts.Count());
                result.Append(topPosts[index]);
                topPosts.RemoveAt(index);
            }

            return result;
        }
    }
}
