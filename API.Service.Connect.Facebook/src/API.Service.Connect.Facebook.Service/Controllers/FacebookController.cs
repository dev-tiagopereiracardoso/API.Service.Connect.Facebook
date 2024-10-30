using API.Service.Connect.Facebook.Domain.Implementation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Service.Connect.Facebook.Service.Controllers
{
	[Route("v1/connect/facebook")]
	[ApiController]
	[ApiExplorerSettings(GroupName = "facebook")]
	public class FacebookController : ControllerBase
	{
		private readonly ILogger<FacebookController> _logger;

		private readonly IFacebookService _facebookService;

		public FacebookController(
				ILogger<FacebookController> logger,
				IFacebookService facebookService
			)
		{
			_logger = logger;
			_facebookService = facebookService;
		}

		[HttpGet("session")]
		public IActionResult CreateSession()
		{
			var Data = _facebookService.CreateSession();

			if (Data == null)
				return BadRequest();

			return Ok(Data);
		}

		[HttpGet("access-token/{Code}")]
		public IActionResult AccessToken(string Code)
		{
			var Data = _facebookService.AccessToken(Code);

			if (Data == null)
				return BadRequest();

			return Ok(Data);
		}


		[HttpGet("refresh-token/{AccessToken}")]
		public IActionResult RefreshToken(string AccessToken)
		{
			var Data = _facebookService.RefreshToken(AccessToken);

			if (Data == null)
				return BadRequest();

			return Ok(Data);
		}

		[HttpGet("user/{AccessToken}")]
		public IActionResult GetDataUser(string AccessToken)
		{
			var Data = _facebookService.GetDataUser(AccessToken);

			if (Data == null)
				return BadRequest();

			return Ok(Data);
		}
	}
}
