using Clients.Data.Repositories;
using Clients.Services.Business.Models.Auth;
using Clients.Services.Business.Services.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Clients.Services.Business.Services
{
    public class AuthService: IAuthService
    {
        private readonly IRepository<User> repository;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthService(IRepository<User> repository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            this.repository = repository;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponse> Login(Login login)
        {
            var user = await repository.FindOneAsync(user => user.NormalizedEmail == login.Email.ToUpper());
            if(user is not null)
            {
                var isPasswordMatched = VerifyPassword(login.Password, user.Salt, user.Password);
                if(isPasswordMatched)
                {
                    user.Password = string.Empty;
                    user.Salt = new byte[0x00];
                    LoginResponse response = new LoginResponse()
                    {
                        User = user,
                        Token = jwtTokenGenerator.GenerateToken(user)
                    };
                    return response;
                }
            }
            throw new ArgumentException("Invalid Login");
        }

        public async Task<User> RegisterUser(User registerUser)
        {
            var hashsalt = EncryptPassword(registerUser.Password);
            var user = new User
            {
                Email = registerUser.Email,
                NormalizedEmail = registerUser.Email.ToUpper(),
                Name = registerUser.Name,
                Password = hashsalt.Hash,
                Salt = hashsalt.Salt
            };
            var duplicatedUser = await repository.FindOneAsync(u => u.NormalizedEmail == registerUser.NormalizedEmail);
            if(duplicatedUser is not null)
            {
                throw new ArgumentException("Duplicated email");
            }
            var createdUser = await repository.InsertAsync(user);
            return createdUser;
        }

        private HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }

        private bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }
    }
}
