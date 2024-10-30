using API.Service.Connect.Facebook.Domain.Implementation.Interfaces;
using API.Service.Connect.Facebook.Models.Output;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace API.Service.Connect.Facebook.Domain.Implementation.Services
{
	public class FacebookService : IFacebookService
	{
		private readonly IConfiguration _configuration;

		private readonly ILogger<FacebookService> _logger;

		public FacebookService(
				IConfiguration configuration,
				ILogger<FacebookService> logger
			)
		{
			_configuration = configuration;
			_logger = logger;
		}

		public AccessTokenFacebookOutput AccessToken(string Code)
		{
			string UrlBase = $"https://graph.facebook.com/v21.0/oauth/access_token";

			var Keys = new Dictionary<string, string>()
			{
				{ "client_id", _configuration["client_id"]! },
				{ "client_secret", _configuration["client_secret"]! },
				{ "redirect_uri", _configuration["redirect_uri"]! },
				{ "code", Code }
			};
			var Data = GetHttpClient(UrlBase, Keys);
			var ResultString = Data.Result!.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<AccessTokenFacebookOutput>(ResultString)!;
		}

		public DataUserOutput GetDataUser(string AccessToken)
		{
			string UrlBase = $"https://graph.facebook.com/me";

			var Keys = new Dictionary<string, string>()
			{
				{ "fields", "id,name,likes,gender,birthday,email" },
				{ "access_token", AccessToken }
			};
			var Data = GetHttpClient(UrlBase, Keys);
			var ResultString = Data.Result!.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<DataUserOutput>(ResultString)!;
		}

		public AccessTokenFacebookOutput RefreshToken(string AccessToken)
		{
			string UrlBase = $"https://graph.facebook.com/v21.0/oauth/access_token";

			var Keys = new Dictionary<string, string>()
			{
				{ "grant_type", "fb_exchange_token" },
				{ "client_id", _configuration["client_id"]! },
				{ "client_secret", _configuration["client_secret"]! },
				{ "fb_exchange_token", AccessToken }
			};

			var Data = GetHttpClient(UrlBase, Keys);
			var ResultString = Data.Result!.Content.ReadAsStringAsync().Result;

			return JsonConvert.DeserializeObject<AccessTokenFacebookOutput>(ResultString)!;
		}

		public CreateSessionOutput? CreateSession()
		{
			string UrlBase = $"https://www.facebook.com/v21.0/dialog/oauth";

			var Keys = new Dictionary<string, string>()
			{
				{ "client_id", _configuration["client_id"]! },
				{ "scope", "public_profile,email" },
				{ "response_type", "code" },
				{ "redirect_uri", _configuration["redirect_uri"]! },
				{ "state", "public_profile,email,user_likes,user_birthday,user_gender" }
			};
			var Data = GetHttpClient(UrlBase, Keys, true);
			var ResultString = Data.Result!.Content.ReadAsStringAsync().Result;

			string padrao = @"URL=/(.*?)\s";

			Match match = Regex.Match(ResultString, padrao);

			if (match.Success)
			{
				string resultado = match.Groups[1].Value;

				return new CreateSessionOutput()
				{
					UrlFacebookConfirmApplication = resultado
				};
			}

			return null;
		}

		private async Task<HttpResponseMessage?> GetHttpClient(string UrlBase, Dictionary<string, string> Keys, bool Headers = false)
		{
			try
			{
				var httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(UrlBase);
				if (Headers)
				{
					httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 OPR/114.0.0.0");
					httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
					httpClient.DefaultRequestHeaders.Add("Host", "www.facebook.com");
					httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "*");
					httpClient.DefaultRequestHeaders.Add("Cookie", "sb=4iIhZ0ufZOaNnBVco_gVS_qE");
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

					var Request = new HttpRequestMessage();

					Request.RequestUri = new Uri(UrlBase);
					Request.Method = HttpMethod.Get;

					foreach (var item in Keys)
					{
						httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
					}

					return await httpClient.SendAsync(Request);
				}
				else
				{
					httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 OPR/114.0.0.0");
					httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
					httpClient.DefaultRequestHeaders.Add("Host", "graph.facebook.com");
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					var Request = new HttpRequestMessage();

					Request.RequestUri = new Uri(UrlBase);
					Request.Method = HttpMethod.Get;

					return await httpClient.GetAsync(QueryHelpers.AddQueryString(UrlBase, Keys!));
				}
			}
			catch (Exception Ex)
			{
				_logger.LogError(Ex.Message);
			}

			return null;
		}
	}
}