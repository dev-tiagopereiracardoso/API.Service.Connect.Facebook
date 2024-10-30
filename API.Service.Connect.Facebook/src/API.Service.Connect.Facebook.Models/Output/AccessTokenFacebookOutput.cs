namespace API.Service.Connect.Facebook.Models.Output
{
	public class AccessTokenFacebookOutput
	{
		public string access_token { set; get; }

		public string token_type { set; get; }

		public int expires_in { set; get; }
	}
}