﻿using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Moviet
{
    public static class SeedData
    {
        public static void Seed(UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IGenreRepository genrerepo,
                                IMovieRepository movierepo,
                                IPostRepository postrepo,
                                IRatingRepository ratingrepo,
                                IInitRepository initrepo,
                                IMovieGenreRepository moviegenrerepo)
        {


            var alreadyInit = SeedInit(initrepo);
            if (alreadyInit)
            {
                return;
            }
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedGenres(genrerepo);
            SeedMovies(movierepo, moviegenrerepo);
            SeedPosts(postrepo, movierepo);
            SeedRatings(ratingrepo);
        }
        private static bool SeedInit(IInitRepository initRepository)
        {
            return initRepository.IsInit();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Roles.Rater).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.Rater
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Roles.ContentManager).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.ContentManager
                };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync(Roles.Administrator).Result)
            {
                var role = new IdentityRole
                {
                    Name = Roles.Administrator
                };
                var result = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            
            // Create Admin
            var admin = new ApplicationUser
            {
                Email = "admin@admin.com",
                UserName = "admin"
            };
            var result = userManager.CreateAsync(admin, "admin").Result;
            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(admin, Roles.Administrator).Wait();
            }

            using (FileStream fs = File.Open("DataInit\\users.csv", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(fs);
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                bool first = true;
                while (!parser.EndOfData)
                {
                    
                    // 0: index, 1: userId, 2: is_content_manager
                    string[] row = parser.ReadFields();
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    var user = new ApplicationUser
                    {
                        Id = row[1],
                        Email = string.Format("user{0}@user.com", row[1]),
                        UserName = string.Format("user{0}", row[1])
                    };

                    userManager.CreateAsync(user, string.Format("user{0}", row[1])).Wait();
                    if (bool.Parse(char.ToUpper(row[2][0]) + row[2].Substring(1)))
                    {
                        userManager.AddToRoleAsync(user, Roles.ContentManager).Wait();
                    }
                    else
                    {
                        userManager.AddToRoleAsync(user, Roles.Rater).Wait();
                    }
                }
            }
        }   

        public static void SeedGenres(IGenreRepository genrerepo)
        {
            using (FileStream fs = File.Open("DataInit\\genres.csv", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(fs);
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                //genrerepo.Clear();
                genrerepo.SetIdentityInsert(true);

                bool first = true;
                while (!parser.EndOfData)
                {

                    // 0: genreId, 1: genre
                    string[] row = parser.ReadFields();

                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    var genre = new Genre
                    {
                        //GenreId = Int32.Parse(row[0]),
                        Name = row[1]
                    };
                    genrerepo.Create(genre);
                }
                genrerepo.SetIdentityInsert(false);

            }
        }

        public static void SeedMovies(IMovieRepository movierepo, IMovieGenreRepository moviegenrerepo)
        {
            using (FileStream fs = File.Open("DataInit\\movies.csv", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var movie_parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(fs);
                movie_parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                movie_parser.SetDelimiters(new string[] { "," });

                //movierepo.Clear();
                //movierepo.SetIdentityInsert(true);

                bool first = true;
                List<Movie> movies = new List<Movie>();
                List<MovieGenre> movieGenres = new List<MovieGenre>();
                while (!movie_parser.EndOfData)
                {
                    
                    //0: index, 1: movieId, 2: title, 3: overview, 4: poster_path, 5: video_key, ** genresId
                    string[] row = movie_parser.ReadFields();
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    var movie = new Movie
                    {
                        MovieId = Int32.Parse(row[1]),
                        Title = row[2],
                        SortDescription = row[3],
                        LongDescription = row[3],
                        PosterPath = Path.Combine("\\posters", row[4].Substring(1)), // ignore 1st char '/'
                        YoutubeId = row[5]
                    };

                    string[] gerne_ids = row[6].Split(',');
                    for (int i = 0; i < gerne_ids.Length; i++)
                    {
                        movieGenres.Add(new MovieGenre
                        {
                            GenreId = Int32.Parse(gerne_ids[i]),
                            MovieId = movie.MovieId

                        });
                    }


                    movies.Add(movie);
                }
                movierepo.InsertBulk(movies);
                moviegenrerepo.InsertBulk(movieGenres);
                //movierepo.SetIdentityInsert(false);

            }
        }

        public static void SeedPosts(IPostRepository postrepo, IMovieRepository movierepo)
        {
            using (FileStream fs = File.Open("DataInit\\posts.csv", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(fs);
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });


                //postrepo.Clear();
                //postrepo.SetIdentityInsert(true);


                bool first = true;
                List<Post> posts = new List<Post>();
                while (!parser.EndOfData)
                {

                    
                    // 0: index, 1: userId, 2: movieId
                    string[] row = parser.ReadFields();
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    var post = new Post
                    {
                        OwnerId = row[1],
                        Movie = movierepo.FindById(Int32.Parse(row[2])),
                        DateCreated = DateTime.Now
                    };
                    posts.Add(post);
                }
                postrepo.InsertBulk(posts);
                //postrepo.SetIdentityInsert(false);

            }
        }

        public static void SeedRatings(IRatingRepository ratingrepo)
        {
            using (FileStream fs = File.Open("DataInit\\ratings.csv", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(fs);
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(new string[] { "," });

                //ratingrepo.SetIdentityInsert(true);

                //ratingrepo.Clear();
                List<Rating> ratings = new List<Rating>();
                var first = true;
                var counter = 0;
                while (!parser.EndOfData)
                {
                    counter++;
                    if (counter % 5000 == 0)
                    {
                        ratingrepo.InsertBulk(ratings);
                        ratings.Clear();
                    }              
                    // 0: index, 1: userId, 2: movieId, 3: rating
                    string[] row = parser.ReadFields();
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    var rating = new Rating
                    {
                        RaterId = row[1],
                        MovieId = Int32.Parse(row[2]),
                        Value = float.Parse(row[3]),
                        DateRated = DateTime.Now
                    };
                    ratings.Add(rating);
                }
                ratingrepo.InsertBulk(ratings);
               // ratingrepo.SetIdentityInsert(false);

            }
        }
    }
}
