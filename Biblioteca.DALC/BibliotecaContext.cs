using Biblioteca.Model;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.DALC
{
	public class BibliotecaContext: DbContext
	{
		public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
			: base (options)
		{

		}

		public DbSet<Libro> Libro { get; set; }
		public DbSet<Categoria> Categoria { get; set; }
		public DbSet<Autor> Autor { get; set; }
	}
}
