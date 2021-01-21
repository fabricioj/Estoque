using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Negocio.Interfaces
{
    public interface IRepositorio<TSource>
    {
        Task<IEnumerable<Mensagem>> Inserir(TSource registro);
        Task<IEnumerable<Mensagem>> Alterar(TSource registro);
        Task<IEnumerable<Mensagem>> Excluir(TSource registro);
    }
}
