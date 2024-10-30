using API.Service.Connect.Facebook.Models.Output;

namespace API.Service.Connect.Facebook.Domain.Implementation.Interfaces
{
	public interface IFacebookService
	{
		CreateSessionOutput? CreateSession();

		AccessTokenFacebookOutput AccessToken(string Code);

		DataUserOutput GetDataUser(string AccessToken);

		AccessTokenFacebookOutput RefreshToken(string AccessToken);
	}
}