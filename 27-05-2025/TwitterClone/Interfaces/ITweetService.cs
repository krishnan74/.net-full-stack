using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterClone.Models;

namespace TwitterClone.Interfaces
{
    public interface ITweetService
    {
        int AddTweet(Tweet Tweet);
        Tweet GetTweetById(int id);
        Tweet UpdateTweet(Tweet Tweet);
        void DeleteTweet(int id);
        // List<Tweet>? SearchTweet( TweetSearchModel TweetSearchModel );
    }
}
