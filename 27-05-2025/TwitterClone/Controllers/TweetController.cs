using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace TwitterClone.Controllers{
    [ApiController]
    [Route("/api/[controller]")]

    public class TweetController: ControllerBase{

        private readonly ITweetService _tweetService;
        public TweetController( ITweetService tweetService )
        {
            _tweetService = tweetService;
        }

        [HttpPost]
        public ActionResult<int> PostTweet([FromBody] Tweet tweet)
        {
            if (tweet == null)
            {
                return BadRequest("Tweet cannot be null");
            }

            var result = _tweetService.AddTweet(tweet);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetTweet), new { id = result }, result);
            }
            return BadRequest("Failed to create tweet");
        }

        // [HttpGet]
        // public ActionResult<List<Tweet>> GetTweets([FromQuery] TweetSearchModel tweetSearchModel)
        // {
        //     var tweets = _tweetService.SearchTweets(tweetSearchModel);
        //     if (tweets == null || tweets.Count == 0)
        //     {
        //         return NotFound("No tweets found");
        //     }
        //     return Ok(tweets);
        // }

        [HttpGet("{id}")]
        public ActionResult<Tweet> GetTweet(int id)
        {
            var tweet = _tweetService.GetTweetById(id);
            if (tweet == null)
            {
                return NotFound();
            }
            return Ok(tweet);
        }
    }
}