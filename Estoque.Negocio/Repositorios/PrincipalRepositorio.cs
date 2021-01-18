using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Estoque.Negocio.Repositorios
{
    public class PrincipalRepositorio<TSource>
    {
        protected readonly IRepositorio<TSource> Repositorio;
        protected readonly ITransacao Transacao;
        protected readonly List<Mensagem> Mensagens;

        public PrincipalRepositorio(IRepositorio<TSource> repositorio, ITransacao transacao)
        {
            Repositorio = repositorio;
            Transacao = transacao;
            Mensagens = new List<Mensagem>();
        }

        protected bool Sucesso()
        {
            return !Mensagens.Any(m => m.Tipo == TipoMensagem.Erro);
        }

        protected bool Falhou()
        {
            return !Sucesso();
        }

        protected IEnumerable<Expression<Func<TSource, object>>> PrepararInclusoes(params Expression<Func<TSource, object>>[] inclusoes)
        {
            return inclusoes;
        }

        protected IEnumerable<Expression<Func<TSource, bool>>> PrepararFiltros(params Expression<Func<TSource, bool>>[] filtros)
        {
            return filtros;
        }
    }
}
