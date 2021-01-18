using Estoque.Negocio.Dominios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Api.Models
{
    public class ProdutoView
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoProduto Tipo { get; set; }
        public TipoStatus Status { get; set; }
        public int? CategoriaId { get; set; }
        public string CategoriaDescricao { get; set; }
    }
}
