using TwitterClone.Interfaces;
using TwitterClone.Models;
using TwitterClone.Repositories;
using TwitterClone.Utils;

namespace TwitterClone.Services{
    public class TwitterService : ITweetService{
        private readonly IRepository<int, Tweet> _tweetRepository;
        private readonly IRepository<int, HashTag> _hashtagRepository;
        private readonly IRepository<int, HashTagTweet> _hashtagTweetRepository;

        public TwitterService(IRepository<int, Tweet> tweetRepository, IRepository<int, HashTag> hashtagRepository, IRepository<int, HashTagTweet> hashtagTweetRepository)
        {
            _tweetRepository = tweetRepository;
            _hashtagRepository = hashtagRepository;
            _hashtagTweetRepository = hashtagTweetRepository;
        }
        
        public int AddTweet(Tweet tweet){
            try{
                
                var result = _tweetRepository.Add(tweet);

                if (result != null)
                {
                    foreach( var word in tweet.Caption.Split(' ') )
                    {
                        if( word.StartsWith("#") && word.Length > 1 )
                        {
                            
                            var hastag_result = _hashtagRepository.Add(new HashTag{Name = word.Substring(1).ToLower()});

                            _hashtagTweetRepository.Add(new HashTagTweet
                            {
                                TweetId = result.Id,
                                HashTagId = hastag_result.Id
                            });   
                        }

                        else{
                            // Handle non-hashtag words if needed
                            Console.WriteLine($"Ignoring word: {word}");
                        }
                    }

                    return result.Id;
                }

                // Return -1 if result is null
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public Tweet GetTweetById(int id){
            try{
                var tweet = _tweetRepository.GetById(id);
                if( tweet != null ){
                    return tweet;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public Tweet UpdateTweet(Tweet tweet){
            try{
                if( tweet == null || string.IsNullOrWhiteSpace(tweet.Caption) ){
                    Console.WriteLine("Tweet Caption cannot be null or empty.");
                    return null;
                }
                var result = _tweetRepository.Update(tweet);
                if (result != null)
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public void DeleteTweet(int id){
            try{
                _tweetRepository.Delete(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}