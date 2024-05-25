using Azure.Core;
using King.Tickets.Application.Services.Integrations.AmadeusApi;
using King.Tickets.Application.Settings;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Json;

namespace King.Tickets.Infrastructure.Services.Integrations.AmadeusApi;

public class AmadeusApiAuthorizantionService : IAmadeusApiAuthorizationService
{
	private readonly HttpClient _httpClient;
	private readonly AmadeusApiSetting _amadeusApiSetting;
	private readonly IMemoryCache _memoryCache;
	private const string AccessToken = "Access_Token";
	public AmadeusApiAuthorizantionService(AmadeusApiSetting amadeusApiSetting, IMemoryCache memoryCache, HttpClient httpClient)
	{
		_amadeusApiSetting = amadeusApiSetting;
		_memoryCache = memoryCache;
		_httpClient = httpClient;
	}

	public async Task<string> GetAccessToken()
	{
		if (!_memoryCache.TryGetValue(AccessToken, out string? accessToken))
		{
			accessToken = await RequestAccessToken();
		}

		return accessToken;
	}
	private async Task<string> RequestAccessToken()
	{
		var request = new HttpRequestMessage(HttpMethod.Post, _amadeusApiSetting.ApiAuthorizationPath);
		request.Content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("grant_type", "client_credentials"),
			new KeyValuePair<string, string>("client_id", _amadeusApiSetting.ApiKey),
			new KeyValuePair<string, string>("client_secret", _amadeusApiSetting.ApiSecret)
		});
		var response = await _httpClient.SendAsync(request);

		var token = await response.Content.ReadFromJsonAsync<AmadeusApiAuthorization>();
		if (token is not null)
		{
			_memoryCache.Set(AccessToken, token.Access_Token);
			return token.Access_Token;
		}

		return "";
	}
}
