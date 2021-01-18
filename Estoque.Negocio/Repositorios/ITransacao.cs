using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Negocio.Utilidades
{
    public interface ITransacao
    {
        Task<IEnumerable<Mensagem>> Salvar();
        Task Desfazer();
    }
}
