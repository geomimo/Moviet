using Microsoft.AspNetCore.Identity;
using Moviet.Contracts;
using Moviet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviet
{
    public static class SeedData
    {
        public static void Seed(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IGenreRepository genrerepo,
                                IPostRepository postrepo)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedGenres(genrerepo);
            SeedPosts(postrepo, genrerepo, userManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if(userManager.FindByNameAsync(Roles.Administrator).Result == null)
            {
                var user = new IdentityUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };

                var result = userManager.CreateAsync(user, "admin").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
                }
            }

            // Create 3 raters.
            for(int i = 1; i <= 3; i++)
            {
                if (userManager.FindByNameAsync("rater" + i.ToString()).Result == null)
                {
                    var user = new IdentityUser
                    {
                        Email = string.Format("rater{0}@rater.com", i),
                        UserName = "rater" + i.ToString()
                    };

                    var result = userManager.CreateAsync(user, "rater" + i.ToString()).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, Roles.Rater).Wait();
                    }
                }
            }

            // Create 3 Content Managers
            for (int i = 1; i <= 3; i++)
            {
                if (userManager.FindByNameAsync("content_manager" + i.ToString()).Result == null)
                {
                    var user = new IdentityUser
                    {
                        Email = string.Format("content_manager{0}@content_manager.com", i),
                        UserName = "content_manager" + i.ToString()
                    };

                    var result = userManager.CreateAsync(user, "content_manager" + i.ToString()).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, Roles.ContentManager).Wait();
                    }
                }
            }
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

        public static void SeedGenres(IGenreRepository genrerepo)
        {
            if (!genrerepo.ExistsByName("Action"))
            {
                var genre = new Genre
                {
                    Name = "Action"
                };
                genrerepo.Create(genre);
            }

            if (!genrerepo.ExistsByName("Adventure"))
            {
                var genre = new Genre
                {
                    Name = "Adventure"
                };
                genrerepo.Create(genre);
            }

            if (!genrerepo.ExistsByName("Comedy"))
            {
                var genre = new Genre
                {
                    Name = "Comedy"
                };
                genrerepo.Create(genre);
            }

            if (!genrerepo.ExistsByName("Drama"))
            {
                var genre = new Genre
                {
                    Name = "Drama"
                };
                genrerepo.Create(genre);
            }

            if (!genrerepo.ExistsByName("Thriller"))
            {
                var genre = new Genre
                {
                    Name = "Thriller"
                };
                genrerepo.Create(genre);
            }
        }
        
        public static void SeedPosts(IPostRepository postrepo, IGenreRepository genrerepo, UserManager<IdentityUser> userManager)
        {
            if (!postrepo.ExistsByMovieTitle("Aladdin"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Comedy")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Adventure")
                    }
                };


                var ratings = new List<Rating>()
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater1").Id,
                        Value = 4.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                        Value = 3.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Aladdin",
                    SortDescription = "A kind-hearted street urchin and a power-hungry Grand Vizier vie for a magic lamp that has the power to make their deepest wishes come true.",
                    LongDescription = "Aladdin who regularly steals to get by with the aid of his pet monkey, Abu. One day while roaming the streets, Aladdin spots a beautiful girl who gets in trouble after giving away bread to children without paying. Aladdin comes to her rescue, and together they get chased by the Royal Guards. After a while they elude their pursuers, and Aladdin takes the girl to his place for some tea. The girl calls herself Dalia, and is the handmaiden to the Princess of Agrabah. She suddenly has to leave as another suitor for the princess, Prince Anders, arrives. Dalia happens to be Princess Jasmine and Dalia is the name of her handmaid and best friend. Meanwhile, The Sultan's trusted councilor, Jafar, is plotting to overthrown the Sultan by getting his hands on the Magic Lamp.",
                    PosterPath = "posters\\05e646d8-4daf-4f02-a2c8-baa4e1496474_aladdin.jpg",
                    YoutubeId = "https://www.youtube.com/embed/_EVlNhmTEPI",
                    PostRemoved = false,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Logan"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Action")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater2").Id,
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Logan",
                    SortDescription = "A mutant child pursued by scientists, comes to Logan for help, he must get her to safety.",
                    LongDescription = "In 2029 the mutant population has shrunken significantly due to genetically modified plants designed to reduce mutant powers and the X-Men have disbanded. Logan, whose power to self-heal is dwindling, has surrendered himself to alcohol and now earns a living as a chauffeur. He takes care of the ailing old Professor X whom he keeps hidden away. One day, a female stranger asks Logan to drive a girl named Laura to the Canadian border. At first he refuses, but the Professor has been waiting for a long time for her to appear. Laura possesses an extraordinary fighting prowess and is in many ways like Wolverine. She is pursued by sinister figures working for a powerful corporation; this is because they made her, with Logan's DNA. A decrepit Logan is forced to ask himself if he can or even wants to put his remaining powers to good use. ",
                    PosterPath = "posters\\0f1f66b2-6a66-4ea0-a5cd-0ac06ca45662_logan.jpg",
                    YoutubeId = "https://www.youtube.com/embed/Div0iP65aZo&t=1s",
                    PostRemoved = false,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Black Panther"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Action")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Adventure")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater3").Id,
                        Value = 3.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 3.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Black Panther",
                    SortDescription = "T'Challa, must step forward to lead his people into a new future and must confront a challenger from his country's past.",
                    LongDescription = "After the events of Captain America: Civil War, Prince T'Challa returns home to the reclusive, technologically advanced African nation of Wakanda to serve as his country's new king. However, T'Challa soon finds that he is challenged for the throne from factions within his own country. When two foes conspire to destroy Wakanda, the hero known as Black Panther must team up with C.I.A. agent Everett K. Ross and members of the Dora Milaje, Wakandan special forces, to prevent Wakanda from being dragged into a world war.",
                    PosterPath = "posters\\50b2f9d7-b3e2-4b98-a668-67f6849b10cd_blackpanther.jpg",
                    YoutubeId = "https://www.youtube.com/embed/xjDjIWPwcPU&t=1s",
                    PostRemoved = true,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Avengers: Endgame"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Action")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Thriller")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Adventure")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater3").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater2").Id,
                        Value = 4.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                        Value = 5.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "Avengers: Endgame",
                    SortDescription = "After the devastating events of Avengers: Infinity War, the universe is in ruins. The Avengers assemble once more in order to reverse Thanos' actions.",
                    LongDescription = "In the opening, Clint Barton is teaching his daughter archery on his secluded farm while his wife prepares a picnic lunch for them. Suddenly, Clint's daughter vanishes and the rest of Clint's family disintegrates, along with half of all life across the universe, the result of Thanos' snapping his fingers after acquiring all six Infinity Stones. Nebula and Tony Stark are stranded in space following their defeat by Thanos on Titan, but are returned to Earth by Carol Danvers and reunited with Natasha Romanoff, Bruce Banner, Steve Rogers, Rocket, Thor, and James Rhodes. The team formulates a plan to steal the Infinity Stones back from Thanos and use them to reverse his actions, but learn upon finding him that he had used the stones a second time to destroy them, preventing their further use. ",
                    PosterPath = "posters\\05e646d8-4daf-4f02-a2c8-baa4e1496474_aladdin.jpg",
                    YoutubeId = "https://www.youtube.com/embed/TcMBFSGVi1c&t=1s",
                    PostRemoved = false,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Baby Driver"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Comedy")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Action")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater1").Id,
                        Value = 3.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                        Value = 4.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "Baby Driver",
                    SortDescription = "After being coerced into working for a crime boss, a young getaway driver finds himself taking part in a heist doomed to fail.",
                    LongDescription = "Baby is a young getaway driver living in Atlanta, Georgia. When he was a child a car accident killed his parents and left him with tinnitus which he blocks out by listening to music on his iPod. He ferries crews of robbers led by a criminal mastermind named Doc in order to pay off a debt he incurred after stealing one of Doc's cars. Between jobs, he creates remixes from snippets of conversations he records and cares for his deaf foster father Joseph. While waiting for his next job Baby meets a young waitress named Debora; the pair quickly bond over their interests in music and fall in love.",
                    PosterPath = "posters\\bd38c682-3a15-4072-bc70-091e991fe76c_babydriver.jpg",
                    YoutubeId = "https://www.youtube.com/embed/D9YZw_X5UzQ",
                    PostRemoved = true,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Joker"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Drama")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater1").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 5.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "Joker",
                    SortDescription = "A mentally troubled comedian Arthur Fleck is disregarded and mistreated by society. He embarks on a downward spiral of revolution and bloody crime. ",
                    LongDescription = "Struggling to make people laugh in grim early-1980s Gotham City, the mentally ill street clown and failed stand-up comedian, Arthur Fleck, wears his smudgy makeup every day to eke out an existence. Mocked, bullied, and above all, marginalised, Fleck's slippery grip on reality will pave the way for a gradual descent into a dark world of unrestrained violence, as a loaded revolver enters the picture offering the long-awaited liberation. Then, the medications stopped working, and troubled Arthur's fierce hatred seems to be the only way out. Is the world prepared for the gloriously malevolent advent of the grinned super-villain, Joker?",
                    PosterPath = "posters\\d1cc320a-902c-499a-8ebf-fc0944547f59_joker.jpg",
                    YoutubeId = "https://www.youtube.com/embed/zAGVQLHvwOY",
                    PostRemoved = false,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }

            if (!postrepo.ExistsByMovieTitle("Dunkirk"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Adventure")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater3").Id,
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                        Value = 4.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Dunkirk",
                    SortDescription = "Allied soldiers from Belgium, the British Empire, and France are surrounded by the German Army and evacuated during a fierce battle in World War II.",
                    LongDescription = "Aladdin who regularly steals to get by with the aid of his pet monkey, Abu. One day while roaming the streets, Aladdin spots a beautiful girl who gets in trouble after giving away bread to children without paying. Aladdin comes to her rescue, and together they get chased by the Royal Guards. After a while they elude their pursuers, and Aladdin takes the girl to his place for some tea. The girl calls herself Dalia, and is the handmaiden to the Princess of Agrabah. She suddenly has to leave as another suitor for the princess, Prince Anders, arrives. Dalia happens to be Princess Jasmine and Dalia is the name of her handmaid and best friend. Meanwhile, The Sultan's trusted councilor, Jafar, is plotting to overthrown the Sultan by getting his hands on the Magic Lamp.",
                    PosterPath = "posters\\d1cc320a-902c-499a-8ebf-fc0944547f59_joker.jpg",
                    YoutubeId = "https://www.youtube.com/embed/F-eMt3SrfFU&t=1s",
                    PostRemoved = false,
                    Genres = genres,
                    Ratings = ratings,
                };

                var post = new Post
                {
                    DateCreated = DateTime.Now,
                    OwnerId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                    Movie = movie
                };

                postrepo.Create(post);
            }
            
        }
    }
}
