using System.Collections.Generic;
using TweetCloneWeb.Entities;

namespace TweetCloneWeb.Models
{
    public class Stream
    {
        public Person User { get; set; }
        public List<Tweet> Tweets { get; set; }
    }
}