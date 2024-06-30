using TwitterThrice.common;
using TwitterThrice.data;

namespace TwitterThrice.domain {
    public class TweetService : ITweetService {

        private readonly ITweetRepository _tweetRepository;
        public TweetService(ITweetRepository tweetRepository) {
            _tweetRepository = tweetRepository;
        }

        public async Task<bool> CreateTweet(Tweet tweet) {
            
            if (tweet == null) {
                throw new ArgumentNullException(nameof(tweet));
            }

            return await _tweetRepository.CreateTweet(tweet);
        }

        public async Task<IEnumerable<TweetDto>> GetRecentTopTenTweets() {
            
            return await _tweetRepository.GetRecentTopTenTweets();
        }
    }
}
