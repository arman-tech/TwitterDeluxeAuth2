using Dapper;
using TwitterThrice.common;

namespace TwitterThrice.data {
    public class TweetRepository : ITweetRepository {

        private readonly DapperDbContext _context;

        public TweetRepository(DapperDbContext dapperDbContext) {
            _context = dapperDbContext;
        }

        public async Task<bool> CreateTweet(Tweet tweet) {
            int rowsAffected = 0;

            //using (var db = _context.CreateConnection()) {

            //    var sql = "INSERT INTO Tweets (Id, MemberId, Message, PostedDate) " +
            //        "VALUES (@Id, @MemberId, @Message, @PostedDate)";

            //    rowsAffected = await db.Exe.ExecuteAsync(sql, tweet);
            //}

            //return rowsAffected > 0;

            var db = _context.CreateConnection();
            var sql = "INSERT INTO Tweets (Id, MemberId, Message, PostedDate) " +
                "VALUES (@Id, @MemberId, @Message, @PostedDate)";
            db.Open();
            rowsAffected = await db.ExecuteAsync(sql, tweet);
            

            return rowsAffected > 0;
        }

        public async Task<IEnumerable<TweetDto>> GetRecentTopTenTweets(int count = 10) {

            var tweets = new List<TweetDto>();

            //using (var db = _context.CreateConnection()) {
            //    var sql = "SELECT TOP 10 * FROM Tweets ORDER BY PostedDate DESC";
            //    var tweets = db.Query<Tweet>(sql);
            //    return Task.FromResult(tweets.AsEnumerable());
            //}

            //using (var connection = new SqlConnection(_connectionString)) {
            //    await connection.OpenAsync();

            //    var sql = "SELECT TOP(@Count) m.name, t.Message, t.PostedDate FROM dbo.Tweets t JOIN dbo.Members m on t.MemberId = m.Id ORDER BY PostedDate DESC";
            //    tweets = (await connection.QueryAsync<TweetDto>(sql, new { Count = count })).ToList();
            //}

            var db = _context.CreateConnection();
            var sql = "SELECT TOP(@Count) m.Username, t.Message, t.PostedDate FROM dbo.Tweets t JOIN dbo.Members m on t.MemberId = m.Id ORDER BY PostedDate DESC";
            tweets = (await db.QueryAsync<TweetDto>(sql, new { Count = count })).ToList();

            return tweets;
        }
    }
}
