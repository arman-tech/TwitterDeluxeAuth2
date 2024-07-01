using TwitterThrice.common;

namespace TwitterThrice.domain {
    public interface IUserService {
        Task<bool> RegisterUser(UserRegistrationDto userRegisteration);
        Task<string?> Login(UserLoginDto userLogin);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserByEmail(string email);
    }
}
