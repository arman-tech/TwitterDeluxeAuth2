using TwitterThrice.common;

namespace TwitterThrice.domain {
    public interface ITweetService {
        Task<IEnumerable<TweetDto>> GetRecentTopTenTweets();
        Task<bool> CreateTweet(Tweet tweet);
    }
}
