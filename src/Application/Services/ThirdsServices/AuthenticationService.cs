using Application.Dtos;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Application.Services;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;


namespace Application.Services.ThirdsServices
{
    public class AuthenticationService : IAuthenticationService
    {
        
        private readonly IUserRepository _userRepository;
        private readonly AuthenticateServiceOptions _options;

        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticateServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }

        private User? ValidateUser(CredentialsRequest credentialsRequest)
        {
            if (string.IsNullOrEmpty(credentialsRequest.Email) || string.IsNullOrEmpty(credentialsRequest.Password))
                return null;

            var user = _userRepository.GetByEmail(credentialsRequest.Email);

            if (user == null) return null;

            if (user.Password == credentialsRequest.Password) return user;

            return null;
        }

       
        public string Authenticate(CredentialsRequest credentialsRequest)
        {
            //Paso 1: Validamos las credenciales
            var user = ValidateUser(credentialsRequest);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User authentication failed");
            }

            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretForKey));
            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString()));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("username", user.Name));
            claimsForToken.Add(new Claim("role", user.Role.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
              _options.Issuer,
              _options.Audience,
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return tokenToReturn.ToString();
        }

        public class AuthenticateServiceOptions
        {
            public const string AuthenticateService = "Authentication";

            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string SecretForKey { get; set; }
        }
    }
}
