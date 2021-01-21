using Estoque.Negocio.Entidades;
using Estoque.Negocio.Interfaces;
using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Negocio.Repositorios
{
    public class CategoriaRepositorio : PrincipalRepositorio<Categoria>, ICategoriaRepositorio
    {
        public CategoriaRepositorio(IBDRepositorio<Categoria> repositorio, ITransacao transacao) : base(repositorio, transacao)
        {

        }

        public Categoria BuscarPorId(int id, bool referenciado)
        {
            var query = Repositorio.IniciarConsulta(referenciado, filters: PrepararFiltros(c => c.Id == id));
            var categoria = query.FirstOrDefault();

            return categoria;
        }

        public IEnumerable<Categoria> Buscar(string pesquisa)
        {
            int.TryParse(pesquisa, out int pesquisaId);
            
            var query = Repositorio.IniciarConsulta(filters: PrepararFiltros((c) => string.IsNullOrEmpty(pesquisa) || c.Id == pesquisaId || c.Descricao.ToUpper().StartsWith(pesquisa.ToUpper())));
            return query.AsEnumerable();
        }

        public async Task<IEnumerable<Mensagem>> Inserir(Categoria categoria)
        {
            if (categoria == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return Mensagens;
            }

            if (string.IsNullOrEmpty(categoria.Descricao))
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Descrição é obrigatória"));
            }

            if (Falhou())
            {
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Inserir(categoria));

            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }

        public async Task<IEnumerable<Mensagem>> Alterar(Categoria categoria)
        {
            if (categoria == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return Mensagens;
            }

            if (categoria.Id == 0)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Id é obrigatório"));
            }

            if (string.IsNullOrEmpty(categoria.Descricao))
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Descrição é obrigatória"));
            }

            if (Falhou())
            {
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Alterar(categoria));
            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }

        public async Task<IEnumerable<Mensagem>> Excluir(Categoria categoria)
        {
            if (categoria == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Excluir(categoria));
            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }
    }
}
