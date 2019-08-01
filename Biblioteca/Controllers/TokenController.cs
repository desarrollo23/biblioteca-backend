using System.Collections.Generic;
using System.Net;
using Biblioteca.Model;
using Biblioteca.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Biblioteca.Controllers
{
	[Route("api/biblioteca/seguridad/")]
    [ApiController]
    public class TokenController : ControllerBase
    {
		private readonly IConfiguration _configuration;

		public TokenController(IConfiguration config)
		{
			_configuration = config;
		}

		[HttpGet("token")]
		[ProducesResponseType(typeof(Token), (int)HttpStatusCode.OK)]
		public ActionResult<IEnumerable<Token>> GetToken(string nombreUnico, string usuario)
		{
			var seguridad = new JwtSeguridad(_configuration);
			var token = seguridad.ConstruirToken(nombreUnico, usuario);

			return Ok(token);
		}
	}
}