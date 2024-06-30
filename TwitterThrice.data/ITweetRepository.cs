using TwitterThrice.common;

namespace TwitterThrice.data {
    public interface ITweetRepository {
        Task<IEnumerable<TweetDto>> GetRecentTopTenTweets(int count = 10);
        Task<bool> CreateTweet(Tweet tweet);
        // Task DeleteTweet(int id);
    }
}
