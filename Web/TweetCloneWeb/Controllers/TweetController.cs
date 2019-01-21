using System.Web.Mvc;
using TweetCloneWeb.Util;
using TweetCloneWeb.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using TweetCloneWeb.Models;

namespace TweetCloneWeb.Controllers
{
    [EntitlementFilter]
    public class TweetController : Controller
    {
        // GET: Tweet Stream home page
        public ActionResult Stream()
        {
            var context = new TweetCloneDB();
            var user = GetUser();
            var streamModel = new Stream
            {
                User = user,
                Tweets = GetTweets(user)
            };
            
            return View(streamModel);
        }

        // Post the tweet
        [HttpPost]
        public ActionResult Update(string message)
        {           
            if (!string.IsNullOrEmpty(message))
            {
                var context = new TweetCloneDB();
                var user = GetUser(context);
                user.Tweets.Add(new Tweet
                {
                    Created = DateTime.Now,
                    Message = message
                });
                context.SaveChanges();
            }

            return RedirectToAction("RefreshBoard");
        }

        // Follow another user
        [HttpPost]
        public ActionResult Follow(string followId)
        {
            if(!string.IsNullOrEmpty(followId))
            {
                var context = new TweetCloneDB();
                var user = GetUser(context);                
                var followUser = context.People.FirstOrDefault(p => p.UserId.Equals(followId, StringComparison.OrdinalIgnoreCase));

                if(followUser != null)
                { 
                    context.Followings.Add(new Following
                    {
                        UserId = followId,
                        FollowingId = user.UserId
                    });
                }
                context.SaveChanges();
            }
            return RedirectToAction("RefreshInfo");
        }

        public ActionResult RefreshBoard()
        {
            var user = GetUser();            
            return PartialView("_TweetBoard", GetTweets(user));
        }

        public ActionResult RefreshInfo()
        {
            return PartialView("_TweetInfo", GetUser());
        }

        private List<Tweet> GetTweets(Person user)
        {
            var context = new TweetCloneDB();

            List<Tweet> tweets = new List<Tweet>();
            if (user == null) return tweets;

            tweets.AddRange(user.Tweets);

            foreach (var follow in user.Followings)
            {
                tweets.AddRange(from tweet in context.Tweets
                                where tweet.UserId.Equals(follow.UserId, StringComparison.InvariantCultureIgnoreCase)
                                select tweet
                                );
            }
            
            return tweets.OrderByDescending(t => t.Created).ToList();
        }

        private Person GetUser(TweetCloneDB context)
        {
            if (context == null) return null;

            String userId = Session["userid"] as String;
            return context.People.FirstOrDefault(p => p.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }

        private Person GetUser()
        {
            var context = new TweetCloneDB();

            String userId = Session["userid"] as String;
            return context.People.FirstOrDefault(p => p.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
        }
    }
}