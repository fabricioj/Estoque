using Estoque.Negocio.Dominios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.Models
{
    public class ProdutoView
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Tipo")]
        public TipoProduto Tipo { get; set; }

        [Display(Name = "Status")]
        public TipoStatus Status { get; set; }

        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }
        
        [Display(Name = "Descrição categoria")]
        public string CategoriaDescricao { get; set; }
    }
}
