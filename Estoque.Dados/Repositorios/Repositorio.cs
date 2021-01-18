using Estoque.Dados.Utilidades;
using Estoque.Negocio.Repositorios;
using Estoque.Negocio.Utilidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Dados.Repositorios
{
    public class Repositorio<TSource> : IRepositorio<TSource> where TSource : class
    {
        private readonly EstoqueContext _EstoqueContext;
        public Repositorio(EstoqueContext estoqueContext)
        {
            _EstoqueContext = estoqueContext;
        }

        public Task<IEnumerable<Mensagem>> Alterar(TSource registro)
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            try
            {
                _EstoqueContext.Set<TSource>().Update(registro);
            }
            catch (Exception ex)
            {
                mensagens.Add(new Mensagem(TipoMensagem.Erro, "Falha durante a inserção"));
            }

            return Task.FromResult<IEnumerable<Mensagem>>(mensagens);
        }

        public Task<IEnumerable<Mensagem>> Excluir(TSource registro)
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            try
            {
                _EstoqueContext.Set<TSource>().Remove(registro);
            }
            catch (Exception ex)
            {
                mensagens.Add(new Mensagem(TipoMensagem.Erro, "Falha durante a exclusão"));
            }

            return Task.FromResult<IEnumerable<Mensagem>>(mensagens);
        }

        public async Task<IEnumerable<Mensagem>> Inserir(TSource registro)
        {
            List<Mensagem> mensagens = new List<Mensagem>();
            try
            {
                await _EstoqueContext.Set<TSource>().AddAsync(registro);
            }
            catch (Exception ex)
            {
                mensagens.Add(new Mensagem(TipoMensagem.Erro, "Falha durante a inserção"));
            }

            return mensagens;
        }

        public IQueryable<TSource> IniciarConsulta(bool referenciado, IEnumerable<Expression<Func<TSource, object>>> includes, IEnumerable<Expression<Func<TSource, bool>>> filters, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy)
        {
            IQueryable<TSource> query = _EstoqueContext.Set<TSource>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    query = query.Where(filter);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!referenciado)
            {
                return query.AsNoTracking();
            }

            return query;
        }

        public async Task<IEnumerable<TSource>> GetAsync(Expression<Func<TSource, bool>> filter = null, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> orderBy = null, params Expression<Func<TSource, object>>[] includes)
        {
            IQueryable<TSource> query = _EstoqueContext.Set<TSource>();
            foreach (Expression<Func<TSource, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }
    }
}
