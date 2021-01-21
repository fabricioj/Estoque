using Estoque.Negocio.Dominios;
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
    public class ProdutoRepositorio : PrincipalRepositorio<Produto>, IProdutoRepositorio
    {
        private ICategoriaRepositorio _RepositorioCategoria;

        public ProdutoRepositorio(IBDRepositorio<Produto> repositorio, ITransacao transacao, ICategoriaRepositorio repositorioCategoria) : base(repositorio, transacao)
        {
            _RepositorioCategoria = repositorioCategoria;
        }

        public Produto BuscarPorId(int id, bool referenciado)
        {
            var query = Repositorio.IniciarConsulta(false, includes: PrepararInclusoes((prod) => prod.Categoria),
                                               filters: PrepararFiltros((prod) => prod.Id == id));
            var produto = query.FirstOrDefault();

            return produto;
        }

        public IEnumerable<Produto> BuscarTodos(bool trazerCategoria)
        {
            var query = Repositorio.IniciarConsulta(false, includes: !trazerCategoria ? default : PrepararInclusoes((prod) => prod.Categoria));
            return query.AsEnumerable();
        }

        public async Task<IEnumerable<Mensagem>> Inserir(Produto produto)
        {
            if (produto == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return Mensagens;
            }

            if (string.IsNullOrEmpty(produto.Descricao))
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Descrição é obrigatória"));
            }

            if (produto.Tipo == TipoProduto.NaoInformado)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Tipo do produto é obrigatório"));
            }

            ValidarCategoria(produto);

            if (Falhou())
            {
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Inserir(produto));
            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }

        public async Task<IEnumerable<Mensagem>> Alterar(Produto produto)
        {
            if (produto == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return Mensagens;
            }

            if (produto.Id == 0)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Id é obrigatório"));
            }

            if (string.IsNullOrEmpty(produto.Descricao))
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Descrição é obrigatória"));
            }

            if (produto.Tipo == TipoProduto.NaoInformado)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Tipo do produto é obrigatório"));
            }

            ValidarCategoria(produto);

            if (Falhou())
            {
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Alterar(produto));
            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }

        public async Task<IEnumerable<Mensagem>> Excluir(Produto produto)
        {
            if (produto == null)
            {
                Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return Mensagens;
            }

            Mensagens.AddRange(await Repositorio.Excluir(produto));
            if (Sucesso())
            {
                Mensagens.AddRange(await Transacao.Salvar());
            }

            return Mensagens;
        }

        private void ValidarCategoria(Produto produto)
        {
            if (produto.CategoriaId != null && produto.CategoriaId != 0)
            {
                var categoriaId = produto.CategoriaId ?? 0;
                var categoria = produto.Categoria;

                if (categoria == null || categoria.Id != categoriaId)
                {
                    categoria = _RepositorioCategoria.BuscarPorId(categoriaId);
                }

                if (categoria == null)
                {
                    Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Categoria não encontrada"));
                }
                else if (categoria.Status == TipoStatus.Inativo)
                {
                    Mensagens.Add(new Mensagem(TipoMensagem.Erro, "Categoria inativa"));
                }
            }
        }
    }
}
