using Biblioteca.DALC;
using Biblioteca.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.BO
{
	public class LibroBo
	{
		private readonly BibliotecaContext _context;

		public LibroBo(BibliotecaContext context)
		{
			_context = context;
		}

		public List<Libro> BuscarLibro(string parametroBusqueda)
		{
			var lstLibros = new List<Libro>();

			var libros = _context.Libro
				.Include(lib => lib.Autor)
				.Include(lib => lib.Categoria)
				.Where(x => string.IsNullOrEmpty(parametroBusqueda) || x.Nombre.Contains(parametroBusqueda) 
				|| x.Categoria.Nombre.Contains(parametroBusqueda) 
				|| x.Autor.Nombres.Contains(parametroBusqueda) || x.Autor.Apellidos.Contains(parametroBusqueda));

			lstLibros.AddRange(libros);

			return lstLibros;
		}
	}
}
