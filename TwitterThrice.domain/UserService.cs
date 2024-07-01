using System.Security.Claims;
using System.Text;
using TwitterThrice.common;
using TwitterThrice.data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using TwitterDeluxeAuth2.common.Exceptions;

namespace TwitterThrice.domain {
    public class UserService : IUserService {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRespository;


        public UserService(IUserRepository userRepository, IConfiguration configuration) {
            _userRespository = userRepository;
            _configuration = configuration;

        }

        public async Task<bool> RegisterUser(UserRegistrationDto userRegistrationDto) {
            return await _userRespository.CreateUser(new User {
                Username = userRegistrationDto.Username,
                Email = userRegistrationDto.Email.Crypt(),
                Password = userRegistrationDto.Password.Crypt()
            });
        }

        public async Task<string?> Login(UserLoginDto userLoginDto) {

            if (userLoginDto == null)
                throw new ArgumentNullException(nameof(userLoginDto));

            // we need to encrypt the email to match the encrypted email in the database
            var encryptedEmail = userLoginDto.Email.Crypt();
            var user = await _userRespository.GetUserByEmail(encryptedEmail);

            if (user != null) {
                var encryptedPassword = userLoginDto.Password.Crypt();                
                if (user.Password == encryptedPassword) {
                    // Generate and return JWT token
                    return GenerateJwtToken(user);
                }
            }

            return null;
        }

        public async Task<User> GetUserByUsername(string username) {

            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var user = await _userRespository.GetUserByUsername(username);

            if (user == null)
                throw new Exception("User not found");

            return user;
        }

        public async Task<User> GetUserByEmail(string email) {

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));

            // encrypt email to match the encrypted email in the database
            var encryptedEmail = email.Crypt();
            var user = await _userRespository.GetUserByEmail(encryptedEmail);

            return user;
        }


        private string GenerateJwtToken(User user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey)) {
                throw new Exception("JWT Key is missing");
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
