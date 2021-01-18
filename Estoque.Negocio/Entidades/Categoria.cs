using Estoque.Negocio.Dominios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estoque.Negocio.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoStatus Status { get; set; }
    }
}
