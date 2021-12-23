using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using dgee2_authapi.Config;
using Microsoft.Extensions.Options;
using dgee2_authapi.Models;

namespace dgee2_authapi
{
	public class JwtHelpers
	{
		readonly JwtKeyConfig primaryKey;
		readonly JwtKeyConfig refreshKey;
		public JwtHelpers(IOptionsSnapshot<JwtConfig> jwtConfig)
		{
			primaryKey = jwtConfig.Value.PrimaryKey;
			refreshKey = jwtConfig.Value.RefreshKey;
		}

		public AuthResponse GenerateTokens(int userId, string email, string username)
		{
			var (accessToken, accessExpires) = GenerateAccessToken(userId, email, username);
			var (refreshToken, refreshExpires) = GenerateRefreshToken(userId, email, username);
			return new AuthResponse(accessToken, accessExpires, refreshToken, refreshExpires);
		}

		public (string Token, DateTime Expires) GenerateAccessToken(int userId, string email, string username)
		{
			var claims = new[] { new Claim("userid", userId.ToString()), new Claim("email", email), new Claim("username", username) };
			return GenerateToken(primaryKey, claims);
		}

		public (string Token, DateTime Expires) GenerateRefreshToken(int userId, string email, string username)
		{
			var claims = new[] { new Claim("userid", userId.ToString()), new Claim("email", email), new Claim("username", username) };
			return GenerateToken(refreshKey, claims);
		}

		private static (string Token, DateTime Expires) GenerateToken(JwtKeyConfig keyConfig, IEnumerable<Claim>? claims = null)
		{
			using var rsa = RSA.Create();
			rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
						source: Convert.FromBase64String(keyConfig.PrivateKey), // Use the private key to sign tokens
						bytesRead: out int _); // Discard the out variable

			var signingCredentials = new SigningCredentials(
				key: new RsaSecurityKey(rsa),
				algorithm: SecurityAlgorithms.RsaSha512Signature // Important to use RSA version of the SHA algo
			)
			{ CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false } }; // This is required due to a bug where it will fail every other time that the object is disposed

			DateTime expires = DateTime.UtcNow.Add(keyConfig.ExpiryTimespan);
			var jwt = new JwtSecurityToken(
				audience: keyConfig.Audience,
				issuer: keyConfig.Issuer,
				claims: claims,
				expires: expires,
				notBefore: DateTime.UtcNow,
				signingCredentials: signingCredentials
			);

			var token = new JwtSecurityTokenHandler().WriteToken(jwt);
			return (token, expires);
		}
	}
}
