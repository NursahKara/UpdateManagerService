using MobilOnayService.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobilOnayService.Helpers
{
    public class TokenHelper
    {
        readonly static string ClassName = "MobilOnayService.Helpers.TokenHelper";
        internal readonly static string SigningKey = "qnR8UCqJggD55PohusaBNviGoOJ67HC6Btry4qXLVZc=";
        internal readonly static string Issuer = "sfi.com.tr";
        internal readonly static string Audience = "sfi.audience";
        internal readonly static int AccessTokenExpireDay = 1;
        internal readonly static int RefresTokenhExpireDay = 10;

        public static IEnumerable<Claim> GenerateClaims(string username, UserData userData)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userData)),
                };
                return claims;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "GenerateClaims");
            }
        }

        public static string GenerateToken(IEnumerable<Claim> claims, out DateTime expires)
        {
            try
            {
                expires = DateTime.UtcNow.AddDays(AccessTokenExpireDay);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
                var jwt = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expires,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return token;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "GenerateToken");
            }
        }

        public static UserData GetUserData(ClaimsPrincipal principal)
        {
            try
            {
                if (principal != null)
                {
                    var identity = (ClaimsIdentity)principal.Identity;
                    var _userData = identity.FindFirst(ClaimTypes.UserData);
                    if (_userData != null)
                        return JsonConvert.DeserializeObject<UserData>(_userData.Value);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Throw(ex, ClassName, "GetUserData");
            }
        }
    }
}
