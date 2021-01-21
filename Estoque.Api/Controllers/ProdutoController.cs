using AutoMapper;
using Estoque.Api.Models;
using Estoque.Negocio.Entidades;
using Estoque.Negocio.Interfaces;
using Estoque.Negocio.Repositorios;
using Estoque.Negocio.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.Api.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    public class ProdutoController : PrincipalController
    {
        private readonly IProdutoRepositorio _ProdutoRepositorio;
        private readonly IMapper _Mapper;

        public ProdutoController(IProdutoRepositorio produtoRepositorio, IMapper mapper)
        {
            _ProdutoRepositorio = produtoRepositorio;
            _Mapper = mapper;
        }

        /// <summary>
        /// Buscar produto por ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Produto/1
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Retorna em "dados" o produto solicitado</returns>
        /// <response code="200">Retorna o produto solicitado</response>
        /// <response code="400">Se nenhumo produto for encontrado</response> 
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public IActionResult BuscarPorId(int id)
        {
            var produto = _ProdutoRepositorio.BuscarPorId(id);
            if (produto == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não encontrado"));
                return RespostaWS();
            }

            return RespostaWS(_Mapper.Map<ProdutoView>(produto));
        }

        /// <summary>
        /// Buscar todos.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Produto/
        ///
        /// </remarks>
        /// <returns>Retorna em "dados" uma lista dos produtos.</returns>
        /// <response code="200">Retornar a lista dos produtos</response>
        [ProducesResponseType(typeof(ApiResposta<IEnumerable<ProdutoView>>), StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult BuscarTodas()
        {
            var produtos = _ProdutoRepositorio.BuscarTodos();

            return RespostaWS(produtos?.Select(produto => _Mapper.Map<ProdutoView>(produto)));
        }

        /// <summary>
        /// Inserir produto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Produto/
        ///     {
        ///        "descricao": "TV 42'",
        ///        "tipo": "revenda",
        ///        "categoriaid": 1,
        ///        "status": "ativo"
        ///     }
        ///     
        /// </remarks>
        /// <param name="produtoView"></param>
        /// <returns>Retorna em "dados" o produto inserido.</returns>
        /// <response code="200">Retorna o produto inserido</response>
        /// <response code="400">Se o produto não for informado ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] ProdutoView produtoView)
        {
            if (produtoView == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return RespostaWS();
            }

            var produto = _Mapper.Map<Produto>(produtoView);

            var mensagens = await _ProdutoRepositorio.Inserir(produto);
            RetornarMensagens(mensagens);

            return RespostaWS(_Mapper.Map<ProdutoView>(produto));
        }

        /// <summary>
        /// Alterar produto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Produto/1
        ///     {
        ///        "descricao": "TV 42' LG SMART",
        ///        "tipo": "revenda",
        ///        "categoriaid": 1,
        ///        "status": "ativo"
        ///     }
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="produtoView"></param>
        /// <returns>Retorna em "dados" o produto alterado.</returns>
        /// <response code="200">Retorna o produto alterado</response>
        /// <response code="400">Se o id do produto não for informado, se ele não for encontrado ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Alterar(int id, [FromBody] ProdutoView produtoView)
        {
            if (id == 0)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return RespostaWS();
            }

            var produto = _ProdutoRepositorio.BuscarPorId(id, true);
            if (produto == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não encontrado"));
                return RespostaWS();
            }

            _Mapper.Map(produtoView, produto, (options) => {
                options.BeforeMap((view, entidade) =>
                {
                    view.Id = entidade.Id;
                });
                options.AfterMap((view, entidade) =>
                {
                    entidade.Categoria = null;
                });
            });

            var mensagens = await _ProdutoRepositorio.Alterar(produto);
            RetornarMensagens(mensagens);

            return RespostaWS(produtoView);
        }

        /// <summary>
        /// Excluir produto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Produto/1
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Retorna em "dados" o produto excluído.</returns>
        /// <response code="200">Retorna o produto excluído</response>
        /// <response code="400">Se o id do produto não for informado, se ele não for encontrado ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<ProdutoView>), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            if (id == 0)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não informado"));
                return RespostaWS();
            }

            var produto = _ProdutoRepositorio.BuscarPorId(id, true);
            if (produto == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Produto não encontrado"));
                return RespostaWS();
            }

            var mensagens = await _ProdutoRepositorio.Excluir(produto);
            RetornarMensagens(mensagens);

            return RespostaWS(_Mapper.Map<ProdutoView>(produto));
        }
    }
}
