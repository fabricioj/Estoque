using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Negocio.Utilidades
{
    public class Mensagem
    {
        public TipoMensagem Tipo { get; set; }
        public string Texto { get; set; }

        public Mensagem(TipoMensagem tipo, string texto)
        {
            Tipo = tipo;
            Texto = texto;
        }

        public Mensagem()
        {

        }
    }
}
