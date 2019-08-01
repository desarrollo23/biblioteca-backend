using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.DALC;
using Biblioteca.Model;
using Biblioteca.BO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Biblioteca.Controllers
{
	[Route("api/biblioteca/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;
		private readonly LibroBo _libroBo;
		private readonly ILogger<LibrosController> _logger;

        public LibrosController(BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _context = context;
			_libroBo = new LibroBo(context);
			_logger = logger;
		}

        [HttpGet]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<IEnumerable<Libro>>> GetLibro()
        {
			_logger.LogInformation("LibrosController: Consultar libros");
			return await _context.Libro.Include(x => x.Categoria).Include(x => x.Autor).ToListAsync();
        }

        [HttpGet("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Libro>> GetLibro(int id)
        {
			_logger.LogInformation($"LibrosController: Obtener libro con id {id}");
			var libro = await _context.Libro.FindAsync(id);

            if (libro == null)
            {
				_logger.LogWarning("LibrosController: Libro no encontrado");
				return NotFound();
			}

            return libro;
        }

        [HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
			_logger.LogInformation($"LibrosController: Editar libro {id}");

			if (id != libro.Id)
            {
                return BadRequest();
            }

			if (!LibroExists(id))
			{
				return NotFound();
			}

			_context.Entry(libro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
				_logger.LogError($"Se presento un error al editar el libro {id}. Error: {ex.Message} - {ex.StackTrace}");
				return StatusCode((int)HttpStatusCode.InternalServerError,
						new { Mensaje = $"Se presento un error al editar el libro '{libro.Nombre}. Error: {ex.Message}" });
			}

            return NoContent();
        }

        [HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<ActionResult<Libro>> PostLibro(Libro libro)
        {
			try
			{
				_context.Libro.Add(libro);
				await _context.SaveChangesAsync();

				return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
			}
			catch (System.Exception ex)
			{
				_logger.LogError($"Se presento un error al crear el libro '{libro.Nombre}'. Error: {ex.Message} - {ex.StackTrace}");

				return StatusCode((int)HttpStatusCode.InternalServerError,
						new { Mensaje = $"Se presento un error al crear el libro '{libro.Nombre}. Error: {ex.Message}" });
			}
           
        }

        [HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Libro>> DeleteLibro(int id)
        {
			_logger.LogInformation($"LibrosController: Eliminar libro {id}");

			var libro = await _context.Libro.FindAsync(id);
            if (libro == null)
            {
				_logger.LogWarning($"LibrosController: El libro {id} no existe");
                return NotFound();
            }

            _context.Libro.Remove(libro);
            await _context.SaveChangesAsync();

            return libro;
        }

        private bool LibroExists(int id)
        {
            return _context.Libro.Any(e => e.Id == id);
        }

		[HttpGet("buscarLibro")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public ActionResult<IEnumerable<Libro>> BuscarLibro(string parametroBusqueda)
		{
			return _libroBo.BuscarLibro(parametroBusqueda);
		}
	}
}
