namespace API.Service.Connect.Facebook.Models.Response
{
	public class FacebookAuthorizationResponse
	{
		public bool IsAuthorized { get; set; }

		public DateTime ExpiresAt { get; set; }

		public string Name { get; set; }
	}
}