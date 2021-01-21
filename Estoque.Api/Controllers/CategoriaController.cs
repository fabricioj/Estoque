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
    public class CategoriaController : PrincipalController
    {
        private readonly ICategoriaRepositorio _CategoriaRepositorio;
        private readonly IMapper _Mapper;

        public CategoriaController(ICategoriaRepositorio categoriaRepositorio, IMapper mapper)
        {
            _CategoriaRepositorio = categoriaRepositorio;
            _Mapper = mapper;
        }

        /// <summary>
        /// Buscar categoria por ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Categoria/1
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Retorna em "dados" a categoria solicitada</returns>
        /// <response code="200">Retorna a categoria solicitada</response>
        /// <response code="400">Se nenhuma categoria for encontrada</response> 
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status400BadRequest)]
        [HttpGet("{id:int}")]
        public IActionResult BuscarPorId(int id)
        {
            var categoria = _CategoriaRepositorio.BuscarPorId(id);
            if (categoria == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não encontrada"));
                return RespostaWS();
            }

            return RespostaWS(_Mapper.Map<CategoriaView>(categoria));
        }

        /// <summary>
        /// Buscar por pesquisa.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Categoria/?pesquisa=CAMA
        ///
        /// </remarks>
        /// <param name="pesquisa"></param>
        /// <returns>Retorna em "dados" uma lista das categorias encontradas conforme o filtro informado (ou todas as categorias se não for informado), seja no código ou descrição.</returns>
        /// <response code="200">Retornar a lista das categorias solicitadas</response>
        [ProducesResponseType(typeof(ApiResposta<IEnumerable<CategoriaView>>), StatusCodes.Status200OK)]
        [HttpGet]
        public IActionResult Buscar(string pesquisa)
        {
            var categorias = _CategoriaRepositorio.Buscar(pesquisa);

            return RespostaWS(categorias?.Select(categoria => _Mapper.Map<CategoriaView>(categoria)));
        }

        /// <summary>
        /// Inserir categoria.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Categoria/
        ///     {
        ///        "descricao": "SALA DE ESTAR",
        ///        "status": "ativo"
        ///     }
        ///     
        /// </remarks>
        /// <param name="categoriaView"></param>
        /// <returns>Retorna em "dados" a categoria inserida.</returns>
        /// <response code="200">Retorna a categoria inserida</response>
        /// <response code="400">Se a categoria não for informada ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CategoriaView categoriaView)
        {
            if (categoriaView == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return RespostaWS();
            }

            var categoria = _Mapper.Map<Categoria>(categoriaView);

            var mensagens = await _CategoriaRepositorio.Inserir(categoria);
            RetornarMensagens(mensagens);

            return RespostaWS(_Mapper.Map<CategoriaView>(categoria));
        }

        /// <summary>
        /// Alterar categoria.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Categoria/1
        ///     {
        ///        "descricao": "SALA DE ESTAR ALTERADA",
        ///        "status": "ativo"
        ///     }
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="categoriaView"></param>
        /// <returns>Retorna em "dados" a categoria alterada.</returns>
        /// <response code="200">Retorna a categoria alterada</response>
        /// <response code="400">Se o id da categoria não for informado, se ela não for encontrada ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Alterar(int id, [FromBody] CategoriaView categoriaView)
        {
            if (id == 0)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return RespostaWS();
            }

            var categoria = _CategoriaRepositorio.BuscarPorId(id, true);
            if (categoria == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não encontrada"));
                return RespostaWS();
            }

            _Mapper.Map(categoriaView, categoria, (options) =>
            {
                options.BeforeMap((view, entidade) =>
                {
                    view.Id = entidade.Id;
                });
            });

            var mensagens = await _CategoriaRepositorio.Alterar(categoria);
            RetornarMensagens(mensagens);

            return RespostaWS(categoriaView);
        }

        /// <summary>
        /// Excluir categoria.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Categoria/1
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Retorna em "dados" a categoria excluída.</returns>
        /// <response code="200">Retorna a categoria excluída</response>
        /// <response code="400">Se o id da categoria não for informado, se ela não for encontrada ou se houver algum erro de validação</response>
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResposta<CategoriaView>), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Excluir(int id)
        {
            if (id == 0)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não informada"));
                return RespostaWS();
            }

            var categoria = _CategoriaRepositorio.BuscarPorId(id, true);
            if (categoria == null)
            {
                RetornarMensagem(new Mensagem(TipoMensagem.Erro, "Categoria não encontrada"));
                return RespostaWS();
            }

            var mensagens = await _CategoriaRepositorio.Excluir(categoria);
            RetornarMensagens(mensagens);

            return RespostaWS(_Mapper.Map<CategoriaView>(categoria));
        }
    }
}
