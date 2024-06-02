using Azure.Core;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Settings;
using King.Tickets.Infrastructure.Services.Integrations.AmadeusApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiAuthorizationService : IAmadeusApiAuthorizationService
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IMemoryCache _memoryCache;
	private const string AccessToken = "AccessToken";
	private const string AccessTokenExpireDateTime = "AccessTokenExpireDateTime";
	private readonly ILogger<AmadeusApiAuthorizationService> _logger;
	public AmadeusApiAuthorizationService(IOptions<AmadeusApiSetting> amadeusApiSetting, IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory, ILogger<AmadeusApiAuthorizationService> logger)
	{
		_amadeusApiSetting = amadeusApiSetting.Value;
		_memoryCache = memoryCache;
		_httpClientFactory = httpClientFactory;
		_logger = logger;
	}

	public async Task<string> GetAccessToken()
	{
		if (!_memoryCache.TryGetValue(AccessToken, out string? accessToken))
		{
			accessToken = await GetAuthozirationToken();
		}
		else if(IsTokenExpired())
		{
            accessToken = await GetAuthozirationToken();
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
	private async Task<string?> GetAuthozirationToken()
	{
		var request = new HttpRequestMessage(HttpMethod.Post, _amadeusApiSetting.ApiAuthorizationPath);
		request.Content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
			new KeyValuePair<string, string>("client_id", _amadeusApiSetting.ApiKey),
			new KeyValuePair<string, string>("client_secret", _amadeusApiSetting.ApiSecret)
		});
		var token = await RequestAuthorizationToken(request);
        return token;
    }
	private async Task<string> RequestAuthorizationToken(HttpRequestMessage request)
	{
        try
        {
            var httpClient = _httpClientFactory.CreateClient();
			using(var response = await httpClient.SendAsync(request))
			{
                _logger.LogInformation($"Response from amadeus api: {request}, {response}");
                var tokenAmadeus = await response.Content.ReadFromJsonAsync<AmadeusApiAuthorization>();
                if (tokenAmadeus == null)
                    throw new Exception("Cannot fetch token from amadeus api");

                _memoryCache.Set(AccessToken, tokenAmadeus.Access_Token);
                _memoryCache.Set(AccessTokenExpireDateTime, DateTime.UtcNow.AddSeconds(tokenAmadeus.Expires_In));
                return tokenAmadeus.Access_Token;
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Requesting for access token failed: {e}");
            throw;
        }
    }
}
