using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Polemonium.Api.Web.Application
{
    public interface IPAuthhentication
    {
        string CreateJwtToken(AppUser user);
    }

    public class PAuthentication : IPAuthhentication
    {
        private PolemoniumOptions options;

        public PAuthentication(IOptions<PolemoniumOptions> options)
        {
            this.options = options.Value;
        }

        public string CreateJwtToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.JwtIssuer,
                audience: options.JwtAudience,
                claims: new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
                },
                expires: DateTime.Now.AddYears(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
