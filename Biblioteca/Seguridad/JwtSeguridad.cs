using Biblioteca.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biblioteca.Seguridad
{
	public class JwtSeguridad
	{
		private readonly IConfiguration _configuration;

		public JwtSeguridad(IConfiguration config)
		{
			_configuration = config;
		}
		public Token ConstruirToken(string nombreUnico, string usuario)
		{

			var nombreUnicoConfig = _configuration["NombreUnico"];
			var usuarioConfig = _configuration["Usuario"];
			var correo = _configuration["Correo"];

			if (!nombreUnico.Equals(nombreUnicoConfig) || !usuario.Equals(usuarioConfig))
				return null;

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.UniqueName, nombreUnicoConfig),
				new Claim("NombreUsuario", usuarioConfig)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var expiration = DateTime.UtcNow.AddMinutes(20);

			JwtSecurityToken token = new JwtSecurityToken(
				issuer: correo,
				audience: correo,
				claims: claims,
				expires: expiration,
				signingCredentials: credentials
			);

			var response = new Token
			{
				TokenCadena = new JwtSecurityTokenHandler().WriteToken(token),
				TiempoExpiracion = expiration
			};

			return response;
		}
	}
}
