using System.Text.Json.Serialization;

namespace dgee2_authapi.Models
{
	public class AuthResponse
	{
		public AuthResponse(string accessToken, DateTime accessTokenExpires, string refreshToken, DateTime refreshTokenExpires)
		{
			AccessToken = accessToken;
			AccessTokenExpires = accessTokenExpires;
			RefreshToken = refreshToken;
			RefreshTokenExpires = refreshTokenExpires;
		}

		[JsonPropertyName("access-token")]
		public string AccessToken { get; }
		[JsonPropertyName("access-token-expires")]
		public DateTime AccessTokenExpires { get; }
		[JsonPropertyName("refresh-token")]
		public string RefreshToken { get; }
		[JsonPropertyName("refresh-token-expires")]
		public DateTime RefreshTokenExpires { get; }
	}
}
