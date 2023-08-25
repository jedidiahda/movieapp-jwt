using Microsoft.IdentityModel.Tokens;
using MovieWebApp.Models;
using MovieWebApp.Repository;
using MovieWebApp.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;

namespace MovieWebApp.Service
{
    public class AccountService
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, 
            IConfiguration config,
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _config = config;
            _mapper = mapper;
        }

        public async Task Save(AccountDTO accountDTO)
        {
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
            Account account = await _accountRepository.Get(accountDTO.Email,accountDTO.Password);
            if (account == null)
            {
                return string.Empty;
            }

            return await GenerateJSONWebToken(account);
        }
    }
}
