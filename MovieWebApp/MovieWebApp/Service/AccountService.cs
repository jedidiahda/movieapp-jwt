using Microsoft.IdentityModel.Tokens;
using MovieWebApp.Models;
using MovieWebApp.Repository;
using MovieWebApp.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MovieWebApp.JwtFeatures;
using MovieWebApp.Utilities;
using Google.Apis.Auth;

namespace MovieWebApp.Service
{
    public class AccountService
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;

        public AccountService(
            JwtHandler jwtHandler,
            IAccountRepository accountRepository, 
            IConfiguration config,
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _config = config;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }

        public async Task Save(AccountDTO accountDTO)
        {
            byte[] salt = null;
            var hasPwd = PasswordHelper.HashPasword(accountDTO.Password, out salt);
            accountDTO.Password = hasPwd;
            accountDTO.Salt = salt;
            var account = _mapper.Map<Account>(accountDTO);
            await _accountRepository.Save(account);
            //return _mapper.Map<AccountDTO>(account);
        }
        private async Task<string> GenerateJSONWebToken(Account account)
        {
            var customer = await _customerRepository.Get(account.Email);
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]??"");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("Active",account.Active.ToString()),
                new Claim("Role",account.Role.ToString()),
                new Claim("Name",customer.FirstName + ' ' + customer.LastName),
                new Claim("CustomerId",customer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public async Task<string> AuthenticateUser(AccountDTO accountDTO)
        {
            Account? account = await _accountRepository.Get(accountDTO.Email??"");
            if (account == null)
            {
                throw new Exception("Invalid user");
            }

            if(PasswordHelper.VerifyPassword(accountDTO.Password, account.Password,account.Salt) == false)
            {
                throw new Exception("Invalid password");
            }

            return await _jwtHandler.GenerateToken(_mapper.Map<AccountDTO>(account));

        }

        public async Task<AccountDTO> Get(string email)
        {
            return _mapper.Map<AccountDTO>(await _accountRepository.Get(email));
        }

        public async Task<string> ExternalLogin(ExternalAuthDTO externalAuth)
        {
            if (externalAuth.Provider == Provider.FACEBOOK)
            {
                var facebook = await _jwtHandler.ValidateFacebookToken(externalAuth);
                if (facebook == null)
                    throw new Exception("Invalid External Authentication.");
            }
            else
            {
                var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
                if (payload == null)
                    throw new Exception("Invalid External Authentication.");
            }

            var user = await Get(externalAuth.Email);
            if (user == null)
            {
                user = new AccountDTO
                {
                    Email = externalAuth.Email,
                    Role = Role.USER,
                    Password = ""
                };

                await Save(user);

                var customer = new CustomerDTO
                {
                    Email = externalAuth.Email,
                    FirstName = externalAuth.FirstName,
                    LastName = externalAuth.LastName,
                    Gender = "M",
                    Address = ""
                };
                await _customerRepository.Save(_mapper.Map<Customer>(customer));
            }

            if (user == null)
                throw new Exception("Invalid External Authentication.");

            //check for the Locked out account

            var token = await _jwtHandler.GenerateToken(user);
            return token.ToString();

        }


    }
}
