using Estoque.Negocio.Dominios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Estoque.Api.Models
{
    public class CategoriaView
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoStatus Status { get; set; }
    }
}
