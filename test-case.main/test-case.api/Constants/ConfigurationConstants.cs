namespace test_case.api.Constants
{
    public class ConfigurationConstants
    {
        public const string ConnectionString = "ConnectionStrings:TestCaseDBConnection";

        public const string Issuer = "JWT:Issuer";
        public const string Audience = "JWT:Audience";
        public const string SecretAccessKey = "JWT:AccessTokenSecret";
        public const string AccessTokenExpiration = "JWT:AccessTokenExpirationMinutes";
        public const string RefreshTokenExpiration = "JWT:RefreshTokenExpirationDays";
    }
}
