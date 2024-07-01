
using Dapper;
using Microsoft.Extensions.Configuration;
using TwitterThrice.common;

namespace TwitterThrice.data {
    public class UserRepository : IUserRepository {

        private readonly DapperDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(DapperDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> CreateUser(User user) {
            using var db = _context.CreateConnection();
            var userExists = await db.QuerySingleOrDefaultAsync<User>("SELECT * FROM Members WHERE Username = @Username", new { user.Username });
            if (userExists != null)
                return false;


            var result = await db.ExecuteAsync("INSERT INTO Members (Id, Username, Email, Password) VALUES (@Id, @Username, @Email, @Password)",
                new { Id = Guid.NewGuid(), user.Username, Email = user.Email, Password = user.Password });

            return result > 0;
        }

        public Task DeleteUser(int id) {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByEmail(string email) {
            using var db = _context.CreateConnection();
            var user = await db.QuerySingleOrDefaultAsync<User>("SELECT * FROM Members WHERE Email = @Email", new { Email = email });

            return user;
        }

        public async Task<User?> GetUserByUsername(string username) {
            using var db = _context.CreateConnection();
            var user = await db.QuerySingleOrDefaultAsync<User>("SELECT * FROM Members WHERE Username = @Username", new { Username = username });

            return user;
        }
    }
}
