using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using Moviet.RecommendationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet.Services.Interfaces
{
    public class RecommendationService : IRecommendationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postrepo;
        private readonly IRatingRepository _ratingrepo;
        private readonly IEvaluationResultsRepository _evalResRepo;

        public RecommendationService(UserManager<ApplicationUser> userManager,
                                     IPostRepository postrepo,
                                     IRatingRepository ratingrepo,
                                     IEvaluationResultsRepository evalResRepo)
        {
            _userManager = userManager;
            _postrepo = postrepo;
            _ratingrepo = ratingrepo;
            _evalResRepo = evalResRepo;
        }

        public List<Post> GetRecommendation(int n, string userId)
        {
            // Find all posts that the users has rated and they are not new
            var ratedPosts = _postrepo.FindAllRatedByUserId(userId).Where(p => !p.IsNew).ToList();

            // Find top 3 genres of the movies.
            var topGenres = GetTopGenres(ratedPosts);

            // Get all posts that have movie with genre in top 3 and have not yet watched.

            var postNotWatched = _postrepo.FindAll().Where(p => p.Movie.Genres.Any(g => topGenres.Contains(g.GenreId))) // top genres
                                                 .Where(p => !ratedPosts.Select(p => p.PostId).ToList().Contains(p.PostId)) // not watched
                                                 .Where(p => !p.IsNew) // Not new
                                                 .ToList(); 

            // Get the movieId list of postNotSeen
            var topMovieIdsNotWatched = postNotWatched.Select(p => p.Movie.MovieId).ToList();

            return GetRecommenedPostsForUser(n, postNotWatched, topMovieIdsNotWatched, userId);

        }

        public void Train()
        {
            var ratings = _ratingrepo.FindAll();
            var results = ModelBuilder.CreateModel(ratings);
            SaveResults(results);

            // Set IsNew = false for all posts -> they have been used for training
            _postrepo.SetIsNewFalse();
            // Set IsNew = false for all users -> they have been used for training
            var users = _userManager.Users.Where(u => u.IsNew);
            foreach(var user in users)
            {
                user.IsNew = false;
                _userManager.UpdateAsync(user);
            }
        }

        private void SaveResults(EvaluationResults results)
        {
            var r = _evalResRepo.Create(results);
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
            return genreCounts.OrderByDescending(d => d.Value).Take(take).Select(d => d.Key).ToList();
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
                result.Add(topPosts[index]);
                topPosts.RemoveAt(index);
            }

            return result;
        }
        private List<Post> GetRecommenedPostsForUser(int n, List<Post> posts, List<int> movieIds, string  userId)
        {
            // Check if user is new -> no trained model for him
            var userIsNew = _userManager.FindByIdAsync(userId).Result.IsNew;
            List<Post> result;
            if (!userIsNew)
            {
                // For each movie not watched, predict its rating
                var movieRatingDict = PredictRatings(movieIds, userId);

                // Get top 40%
                movieIds = movieRatingDict.OrderByDescending(d => d.Value)
                                                 .Take((int)Math.Round(movieRatingDict.Count * 0.4, 0))
                                                 .Select(d => d.Key)
                                                 .ToList();
            }
            // If user is new, just pick random non watched movies
            result = GetRandomPosts(n, posts, movieIds);

            return result;

        }
    }
}
