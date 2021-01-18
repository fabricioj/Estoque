using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Negocio.Utilidades
{
    public class ApiResposta<TSource>
    {
        public bool sucesso { get; set; }
        public TSource dados { get; set; }
        public IEnumerable<Mensagem> mensagens { get; set; }
    }
}
