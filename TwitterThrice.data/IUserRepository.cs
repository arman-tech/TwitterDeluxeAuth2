using TwitterThrice.common;

namespace TwitterThrice.data {
    public interface IUserRepository {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserByEmail(string email);
        Task<bool> CreateUser(User user);
        Task DeleteUser(int id);
    }
}
