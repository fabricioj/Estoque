using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Negocio.Repositorios
{
    public interface IRepositorio<TSource>
    {
        Task<IEnumerable<Mensagem>> Inserir(TSource registro);
        Task<IEnumerable<Mensagem>> Alterar(TSource registro);
        Task<IEnumerable<Mensagem>> Excluir(TSource registro);

        IQueryable<TSource> IniciarConsulta(bool referenciado = false, IEnumerable<Expression<Func<TSource, object>>> includes = default, IEnumerable<Expression<Func<TSource, bool>>> filter = default, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy = default);
    }
}
