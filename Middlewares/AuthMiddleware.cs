using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CustyUpBankingApi.Middlewares
{
	public class AuthMiddleware : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		private readonly IConfiguration _configuration;
		public AuthMiddleware(
			IOptionsMonitor<AuthenticationSchemeOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IConfiguration configuration
			)
			: base(options, logger, encoder, clock)
		{
			_configuration = configuration;
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization"))
			{
				return AuthenticateResult.Fail("Authorization header missing.");
			}

			string providedApiKey = Request.Headers["Authorization"];

			if (providedApiKey == _configuration["CustyUpApiKey"])
			{
				var claims = new[] { new Claim(ClaimTypes.Name, "Admin") };
				var identity = new ClaimsIdentity(claims, Scheme.Name);
				var principal = new ClaimsPrincipal(identity);
				var ticket = new AuthenticationTicket(principal, Scheme.Name);

				return AuthenticateResult.Success(ticket);
			}

			return AuthenticateResult.Fail("Invalid API key.");
		}
	}
}
