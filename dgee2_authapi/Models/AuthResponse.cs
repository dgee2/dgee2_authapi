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

		public string AccessToken { get; }
		public DateTime AccessTokenExpires { get; }
		public string RefreshToken { get; }
		public DateTime RefreshTokenExpires { get; }
	}
}
