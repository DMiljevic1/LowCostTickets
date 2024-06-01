using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Settings;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiAuthorizationService : IAmadeusApiAuthorizationService
{
	private readonly HttpClient _httpClient;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IMemoryCache _memoryCache;
	private const string AccessToken = "AccessToken";
	private const string AccessTokenExpireDateTime = "AccessTokenExpireDateTime";
	private readonly ILogger<AmadeusApiAuthorizationService> _logger;
	public AmadeusApiAuthorizationService(IOptions<AmadeusApiSetting> amadeusApiSetting, IMemoryCache memoryCache, 
		HttpClient httpClient, ILogger<AmadeusApiAuthorizationService> logger)
	{
		_amadeusApiSetting = amadeusApiSetting.Value;
		_memoryCache = memoryCache;
		_httpClient = httpClient;
		_logger = logger;
	}

	public async Task<string> GetAccessToken()
	{
		if (!_memoryCache.TryGetValue(AccessToken, out string? accessToken))
		{
			_logger.LogDebug("Access token is null. Fetching token...");
			accessToken = await RequestAccessToken();
			_logger.LogDebug("Access token received: {@accessToken}", accessToken);
		}
		else if(IsTokenExpired())
		{
			_logger.LogDebug("Access token is expired. Fetching new one...");
            accessToken = await RequestAccessToken();
            _logger.LogDebug("Access token received: {@accessToken}", accessToken);
        }

		return accessToken;
	}
	private bool IsTokenExpired()
	{
        if (_memoryCache.TryGetValue(AccessTokenExpireDateTime, out DateTime? accessTokenExpireDateTime))
		{
			return DateTime.UtcNow >= accessTokenExpireDateTime;
		}
		return true;
    }
	private async Task<string?> RequestAccessToken()
	{
		var request = new HttpRequestMessage(HttpMethod.Post, _amadeusApiSetting.ApiAuthorizationPath);
		request.Content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
			new KeyValuePair<string, string>("client_id", _amadeusApiSetting.ApiKey),
			new KeyValuePair<string, string>("client_secret", _amadeusApiSetting.ApiSecret)
		});
		try
		{
            var response = await _httpClient.SendAsync(request);
			_logger.LogDebug("Response from amadeus api: {@request}, {@response}", request, response);
            var token = await response.Content.ReadFromJsonAsync<AmadeusApiAuthorization>();
            if (token != null)
            {
                _memoryCache.Set(AccessToken, token.Access_Token);
                _memoryCache.Set(AccessTokenExpireDateTime, DateTime.UtcNow.AddSeconds(token.Expires_In));
                return token.Access_Token;
            }
        }
		catch (Exception e)
		{
			_logger.LogError("Requesting for access token failed: {@e}", e);
			throw;
		}

		return null;
	}
}
