using Estoque.Negocio.Dominios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estoque.Negocio.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoProduto Tipo { get; set; }
        public int? CategoriaId { get; set; }
        public TipoStatus Status { get; set; }
        public Categoria Categoria { get; set; }
    }
}
