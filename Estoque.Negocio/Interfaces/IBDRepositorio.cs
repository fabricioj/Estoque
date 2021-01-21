using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Estoque.Negocio.Interfaces
{
    public interface IBDRepositorio<TSource>: IRepositorio<TSource>
    {
        IQueryable<TSource> IniciarConsulta(bool referenciado = false, IEnumerable<Expression<Func<TSource, object>>> includes = default, IEnumerable<Expression<Func<TSource, bool>>> filters = default, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy = default);
    }
}
