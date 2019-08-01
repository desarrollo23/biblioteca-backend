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
	[Route("api/biblioteca/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly BibliotecaContext _context;
		private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(BibliotecaContext context, ILogger<CategoriasController> logger)
        {
            _context = context;
			_logger = logger;
        }

        [HttpGet]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoria()
        {
            return await _context.Categoria.ToListAsync();
        }

        [HttpGet("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return BadRequest();
            }

			if (!CategoriaExists(id))
			{
				return NotFound();
			}

			_context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
				_logger.LogError($"Se presento un error al editar la categoria {id}. Error: {ex.Message} - {ex.StackTrace}");

				return StatusCode((int)HttpStatusCode.InternalServerError,
						new { Mensaje = $"Se presento un error al editar la categoria '{categoria.Nombre}. Error: {ex.Message}" });
			}

            return NoContent();
        }

        [HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[ProducesResponseType((int)HttpStatusCode.InternalServerError)]
		public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
			try
			{
				_context.Categoria.Add(categoria);
				await _context.SaveChangesAsync();

				return CreatedAtAction("GetCategoria", new { id = categoria.Id }, categoria);
			}
			catch (System.Exception ex)
			{
				_logger.LogError($"Se presento un error al crear la categoria '{categoria.Nombre}'. Error: {ex.Message} - {ex.StackTrace}");

				return StatusCode((int)HttpStatusCode.InternalServerError,
										new { Mensaje = $"Se presento un error al crear la categoria '{categoria.Nombre}. Error: {ex.Message}" });
			}
           
        }

        [HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<Categoria>> DeleteCategoria(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
