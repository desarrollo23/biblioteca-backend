using System;
using System.Runtime.Serialization;

namespace Biblioteca.Model
{
	[DataContract]
	public class Token
	{
		[DataMember(Name = "tokenCadena")]
		public string TokenCadena { get; set; }

		[DataMember(Name = "tiempoExpiracion")]
		public DateTime TiempoExpiracion { get; set; }
	}
}
