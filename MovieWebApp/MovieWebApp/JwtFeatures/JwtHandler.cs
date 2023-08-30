using AutoMapper;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using MovieWebApp.DTO;
using MovieWebApp.Repository;
using MovieWebApp.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieWebApp.JwtFeatures
{
    public  class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly CustomerService _customerService;
        private readonly IHttpClientFactory _httpClientFactory;



        public JwtHandler(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerService = new CustomerService(mapper,customerRepository);
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("Jwt");
            _goolgeSettings = _configuration.GetSection("Google");
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GenerateToken(AccountDTO accountDTO)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(accountDTO);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string?>() { _goolgeSettings.GetSection("ClientId").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

            return payload;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("Key").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(AccountDTO accountDTO)
        {
            var customer = await _customerService.Get(accountDTO.Email);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, accountDTO.Email),
                new Claim("Active",accountDTO.Active.ToString()),
                new Claim("Role",accountDTO.Role.ToString()),
                new Claim("Name",customer.FirstName + " " + customer.LastName),
                new Claim("CustomerId",customer.Id.ToString()),
            };

            //claims.Add(new Claim(ClaimTypes.Role, accountDTO.Role));

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["Issuer"],
                audience: _jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["ExpiryInMinutes"])),signingCredentials: signingCredentials);

            return tokenOptions;
        }

        public async Task<FacebookTokenValidationResult> ValidateFacebookToken(ExternalAuthDTO request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var appAccessTokenResponse = await httpClient.GetFromJsonAsync<FacebookAppAccessTokenResponse>($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["Facebook:AppId"]}&client_secret={_configuration["Facebook:ClientSecret"]}&grant_type=client_credentials");
            var response =
                await httpClient.GetFromJsonAsync<FacebookTokenValidationResult>(
                    $"https://graph.facebook.com/debug_token?input_token={request.IdToken}&access_token={appAccessTokenResponse!.AccessToken}");


            return response;
        }

    }
}

