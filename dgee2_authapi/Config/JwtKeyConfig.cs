namespace dgee2_authapi.Config
{
    public class JwtKeyConfig
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public TimeSpan ExpiryTimespan { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}