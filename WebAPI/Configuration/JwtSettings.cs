namespace WebAPI.Configuration
{
    /// <summary>
    /// Represents the JWT (JSON Web Token) settings required for configuration.
    /// </summary>
    public class JwtSettings
    {
        private static readonly string _section = "JwtSettings";

        private readonly IConfiguration _configuration;

        /// <summary>
        /// Gets the issuer value for the JWT.
        /// </summary>
        public string? Issuer => _configuration.GetSection(_section).GetSection("ValidIssuer").Value;

        /// <summary>
        /// Gets the audience value for the JWT.
        /// </summary>
        public string? Audience => _configuration.GetSection(_section).GetSection("ValidAudience").Value;

        /// <summary>
        /// Gets the secret value for the JWT.
        /// </summary>
        public string? Secret => _configuration.GetSection(_section).GetSection("Secret").Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtSettings"/> class.
        /// </summary>
        /// <param name="configuration">The configuration object to retrieve settings from.</param>
        public JwtSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}