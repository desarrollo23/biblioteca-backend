using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Model
{
	public class Libro
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public int AutorId { get; set; }
		public int CategoriaId { get; set; }

		public Autor Autor { get; set; }

		public Categoria Categoria { get; set; }
	}
}
