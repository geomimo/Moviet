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
                    YoutubeId = "https://www.youtube.com/embed/Div0iP65aZo",
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
                    YoutubeId = "https://www.youtube.com/embed/xjDjIWPwcPU",
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
                    YoutubeId = "https://www.youtube.com/embed/TcMBFSGVi1c",
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
                    LongDescription = "May/June 1940. Four hundred thousand British and French soldiers are hole up in the French port town of Dunkirk. The only way out is via sea, and the Germans have air superiority, bombing the British soldiers and ships without much opposition. The situation looks dire and, in desperation, Britain sends civilian boats in addition to its hard-pressed Navy to try to evacuate the beleaguered forces. This is that story, seen through the eyes of a soldier amongst those trapped forces, two Royal Air Force fighter pilots, and a group of civilians on their boat, part of the evacuation fleet.",
                    PosterPath = "posters\\d1cc320a-902c-499a-8ebf-fc0944547f59_joker.jpg",
                    YoutubeId = "https://www.youtube.com/embed/F-eMt3SrfFU",
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

            if (!postrepo.ExistsByMovieTitle("Parasite"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Thriller")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Drama")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Comedy")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater2").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                        Value = 5.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "Parasite",
                    SortDescription = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.",
                    LongDescription = "Jobless, penniless, and, above all, hopeless, the unmotivated patriarch, Ki-taek, and his equally unambitious family--his supportive wife, Chung-sook; his cynical twentysomething daughter, Ki-jung, and his college-age son, Ki-woo--occupy themselves by working for peanuts in their squalid basement-level apartment. Then, by sheer luck, a lucrative business proposition will pave the way for an ingeniously insidious scheme, as Ki-woo summons up the courage to pose as an English tutor for the teenage daughter of the affluent Park family. Now, the stage seems set for an unceasing winner-take-all class war. How does one get rid of a parasite?",
                    PosterPath = "posters\\c2a02983-a39c-4023-9c34-54248bc625f5_parasite.jpg",
                    YoutubeId = "https://www.youtube.com/embed/5xH0HfJHsaY",
                    PostRemoved = false,
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

            if (!postrepo.ExistsByMovieTitle("1917"))
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
                        Value = 4.5F
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
                    Title = "1917",
                    SortDescription = "Two soldiers are assigned to race against time and deliver a message that will stop 1,600 men from walking straight into a deadly trap.",
                    LongDescription = "British trenches somewhere in France. World war has been going on for the third year, heroic illusions have dissipated; general mood - boredom and fatigue. Stuff the belly, sleep, return home to Christmas Eve. On another quiet day, when nothing happens, two young soldiers, Blake and Schofield, are summoned to the general, who instructs them to send an important message to Colonel MacKenzie in the Second Devonshire Battalion, whose telephone connection was cut off by the enemy.",
                    PosterPath = "posters\\74a4c4b5-5a94-453a-9b0b-b891cd85d11a_1917.jpg",
                    YoutubeId = "https://www.youtube.com/embed/YqNYrYUiMfg",
                    PostRemoved = false,
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

            if (!postrepo.ExistsByMovieTitle("The Godfather"))
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
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 4.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "The Godfather",
                    SortDescription = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                    LongDescription = "The Godfather 'Don' Vito Corleone is the head of the Corleone mafia family in New York. He is at the event of his daughter's wedding. Michael, Vito's youngest son and a decorated WW II Marine is also present at the wedding. Michael seems to be uninterested in being a part of the family business. Vito is a powerful man, and is kind to all those who give him respect but is ruthless against those who do not. But when a powerful and treacherous rival wants to sell drugs and needs the Don's influence for the same, Vito refuses to do it. What follows is a clash between Vito's fading old values and the new ways which may cause Michael to do the thing he was most reluctant in doing and wage a mob war against all the other mafia families which could tear the Corleone family apart.",
                    PosterPath = "posters\\ef3350f0-ff28-4ff2-8ea8-840398b6fcdf_thegodfather.jpg",
                    YoutubeId = "https://www.youtube.com/embed/sY1S34973zA",
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

            if (!postrepo.ExistsByMovieTitle("Uncut Gems"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Drama")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Thriller")
                    }
                };

                var ratings = new List<Rating>
                {
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater1").Id,
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 4.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Uncut Gems",
                    SortDescription = "With his debts mounting and angry collectors closing in, a fast-talking New York City jeweler risks everything in hope of staying afloat and alive.",
                    LongDescription = "In New York City's chaotic and corrupt diamond market, the gemstone dealer and gambling addict, Howard Ratner, is owing money to almost everyone, including his brother-in-law, Arno. A serial adulterer, incorrigible gambler, and all-around con artist, Howard thinks he can finally get out of the tight spot when he gets his hands on a block of rare Ethiopian black opal--the means to clear his ever-rising debts. But, the star Boston Celtics player, Kevin Garnett, has already taken a shine to the precious uncut gem. Can Ratner juggle his family, a pair of thuggish collectors, his work, and his master plan before things get nasty?",
                    PosterPath = "posters\\bc765487-c776-4aca-9cbb-65181480d31b_uncutgems.jpg",
                    YoutubeId = "https://www.youtube.com/embed/vTfJp2Ts9X8",
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

            if (!postrepo.ExistsByMovieTitle("Inception"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Adventure")
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
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                        Value = 2.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "Inception",
                    SortDescription = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.",
                    LongDescription = "Dom Cobb is a skilled thief, the absolute best in the dangerous art of extraction, stealing valuable secrets from deep within the subconscious during the dream state, when the mind is at its most vulnerable. Cobb's rare ability has made him a coveted player in this treacherous new world of corporate espionage, but it has also made him an international fugitive and cost him everything he has ever loved. Now Cobb is being offered a chance at redemption. One last job could give him his life back but only if he can accomplish the impossible, inception. Instead of the perfect heist, Cobb and his team of specialists have to pull off the reverse: their task is not to steal an idea, but to plant one. If they succeed, it could be the perfect crime.",
                    PosterPath = "posters\\5159502d-3f50-4202-9343-c89e91ab8784_inception.jpg",
                    YoutubeId = "https://www.youtube.com/embed/YoHD9XEInc0",
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

            if (!postrepo.ExistsByMovieTitle("Jojo Rabbit"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Comedy")
                    },
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
                        Value = 4.5F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager2").Id,
                        Value = 4.5F
                    }
                };

                var movie = new Movie
                {
                    Title = "Jojo Rabbit",
                    SortDescription = "A young boy in Hitler's army finds out his mother is hiding a Jewish girl in their home.",
                    LongDescription = "In the waning months of the Third Reich, the unpopular ten-year-old German boy, Johannes 'Jojo' Betzler, can't wait to join the ranks of the Nazi Party's youth organisation, during an intense training weekend that guarantees to separate the men from the boys. Massively into swastikas and ready to give up his life for his megalomaniac idol, Adolf Hitler, instead, the Führer's tiny number one fan gets kicked out of the Hitler Youth after a disastrous first assignment in front of his peers--an ignominious defeat that earns Jojo an equally degrading nickname. Now, with nothing but time on his hands, Johannes is in for a rude awakening when he accidentally unearths his progressive mother's well-hidden secret and comes face-to-face with a shocking new reality so much different from the hypnotic indoctrinations he's absorbed.",
                    PosterPath = "posters\\01a61fe0-494a-42ac-a1a5-1c5427b18a5b_jojorabbit.jpg",
                    YoutubeId = "https://www.youtube.com/embed/ZkKaDXapi1o",
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

            if (!postrepo.ExistsByMovieTitle("The Imitation Game"))
            {
                var genres = new List<MovieGenre>
                {
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Drama")
                    },
                    new MovieGenre
                    {
                        GenreId = genrerepo.GetIdByName("Thriller")
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
                        RaterId = userManager.GetUsersInRoleAsync(Roles.Rater).Result.SingleOrDefault(r => r.UserName == "rater2").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager1").Id,
                        Value = 5.0F
                    },
                    new Rating
                    {
                        DateRated = DateTime.Now,
                        RaterId = userManager.GetUsersInRoleAsync(Roles.ContentManager).Result.SingleOrDefault(r => r.UserName == "content_manager3").Id,
                        Value = 5.0F
                    }
                };

                var movie = new Movie
                {
                    Title = "The Imitation Game",
                    SortDescription = "During World War II, the English mathematical genius Alan Turing tries to crack the German Enigma code with help from fellow mathematicians.",
                    LongDescription = "In 1951, two policemen, Nock and Staehl, investigate mathematician Alan Turing following an apparent break-in at his home. During an interrogation by Nock, Turing tells of his time working at Bletchley Park during the Second World War. In 1927, young Turing, unhappily bullied at boarding school, develops a friendship with Christopher Morcom, who sparks his interest in cryptography. Turing develops romantic feelings for him, but Christopher soon dies from tuberculosis. When Britain declares war on Germany in 1939, Turing travels to Bletchley Park. Under the direction of Commander Alastair Denniston, he joins the cryptography team of Hugh Alexander, John Cairncross, Peter Hilton, Keith Furman and Charles Richards, who try to decrypt the Enigma machine that the Nazis use to send coded messages.",
                    PosterPath = "posters\\80054248-8747-4832-b7de-5998554cae70_imitationgame.jpg",
                    YoutubeId = "https://www.youtube.com/embed/nuPZUUED5uk",
                    PostRemoved = false,
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
        }
    }
}
