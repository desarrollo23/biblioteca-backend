using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.DALC;
using Biblioteca.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Biblioteca.Controllers
{
	[Route("api/biblioteca/autores")]
    [ApiController]
    public class AutorsController : ControllerBase
    {
        private readonly BibliotecaContext _context;
		private readonly ILogger<AutorsController> _logger;

        public AutorsController(BibliotecaContext context, ILogger<AutorsController> logger)
        {
            _context = context;
			_logger = logger;
        }

        [HttpGet]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<IEnumerable<Autor>>> GetAutor()
        {
			return await _context.Autor.ToListAsync();
        }

        [HttpGet("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Autor>> GetAutor(int id)
        {
            var autor = await _context.Autor.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

			if (!AutorExists(id))
			{
				return NotFound();
			}

			_context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
				_logger.LogInformation($"Se presento un error al editar el autor {autor.Nombres}. Error: {ex.Message} - {ex.StackTrace} ");

				return StatusCode((int)HttpStatusCode.InternalServerError,
						new { Mensaje = $"Se presento un error al editar el autor '{autor.Nombres}. Error: {ex.Message}" });
			}

            return NoContent();
        }

        [HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
			try
			{
				_context.Autor.Add(autor);
				await _context.SaveChangesAsync();

				return CreatedAtAction("GetAutor", new { id = autor.Id }, autor);
			}
			catch (System.Exception ex)
			{
				_logger.LogInformation($"Se presento un error al crear el autor {autor.Nombres}. Error: {ex.Message} - {ex.StackTrace} ");

				return StatusCode((int)HttpStatusCode.InternalServerError,
										new { Mensaje = $"Se presento un error al crear el autor '{autor.Nombres}. Error: {ex.Message}" });
			}
           
        }

        [HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Autor>> DeleteAutor(int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();

            return autor;
        }

        private bool AutorExists(int id)
        {
            return _context.Autor.Any(e => e.Id == id);
        }
    }
}
