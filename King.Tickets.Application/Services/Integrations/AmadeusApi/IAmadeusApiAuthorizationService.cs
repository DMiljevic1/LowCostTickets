namespace King.Tickets.Application.Services.Integrations.AmadeusApi;

public interface IAmadeusApiAuthorizationService
{
	Task<string> GetAccessToken();
}
