namespace dgee2_authapi.Config
{
    public class JwtConfig
    {
        public const string ConfigName = "jwt";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public JwtKeyConfig PrimaryKey { get; set; }
        public JwtKeyConfig RefreshKey { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
