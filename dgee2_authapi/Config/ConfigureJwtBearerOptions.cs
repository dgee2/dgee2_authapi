using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace dgee2_authapi.Config
{
	public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
	{
		private readonly JwtConfig config;

		public ConfigureJwtBearerOptions(IOptions<JwtConfig> config)
		{
			this.config = config.Value;
		}

		public void Configure(JwtBearerOptions options)
		{
			throw new NotImplementedException();
		}

		public void Configure(string name, JwtBearerOptions options)
		{
			var keyConfig = name switch
			{
				"refresh-token" => config.RefreshKey,
				"primary-token" => config.PrimaryKey,
				_ => throw new ArgumentOutOfRangeException(nameof(name))
			};

			RSA rsa = RSA.Create();
			rsa.ImportRSAPublicKey(Convert.FromBase64String(keyConfig.PublicKey), out _);

			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				IssuerSigningKey = new RsaSecurityKey(rsa),
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateLifetime = true,
				RequireExpirationTime = true,
				ClockSkew = TimeSpan.Zero,
				ValidateIssuerSigningKey = true,
				ValidIssuer = keyConfig.Issuer,
				ValidAudience = keyConfig.Audience,
				CryptoProviderFactory = new CryptoProviderFactory()
				{
					CacheSignatureProviders = false
				},
			};
		}
	}
}
