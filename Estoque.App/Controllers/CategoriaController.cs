using Estoque.App.Models;
using Estoque.App.Utilities;
using Estoque.Negocio.Dominios;
using Estoque.Negocio.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.Controllers
{
    public class CategoriaController : PrincipalController
    {
        private readonly IEstoqueApi _EstoqueApi;

        public CategoriaController(IEstoqueApi estoqueApi)
        {
            _EstoqueApi = estoqueApi;
        }

        //GET: Categoria
        public async Task<IActionResult> Index()
        {
            ApiResposta<IEnumerable<CategoriaView>> categorias = new ApiResposta<IEnumerable<CategoriaView>>();
            try
            {
                categorias = await _EstoqueApi.CategoriaBuscar();
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }
            return View(categorias.dados);
        }

        //GET: Categoria/Create
        public IActionResult Create()
        {
            var categoria = new CategoriaView();
            categoria.Status = TipoStatus.Ativo;
            return _RetornarForm(Operacao.Create, categoria);
        }

        //GET: Categoria/Details
        public async Task<IActionResult> Details(int id)
        {
            ApiResposta<CategoriaView> apiRetorno = new ApiResposta<CategoriaView>();
            try
            {
                apiRetorno = await _EstoqueApi.CategoriaBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return _RetornarForm(Operacao.Details, apiRetorno.dados);
        }

        //GET: Categoria/Edit
        public async Task<IActionResult> Edit(int id)
        {
            ApiResposta<CategoriaView> apiRetorno = new ApiResposta<CategoriaView>();
            try
            {
                apiRetorno = await _EstoqueApi.CategoriaBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return _RetornarForm(Operacao.Edit, apiRetorno.dados);
        }

        //GET: Categoria/Delete
        public async Task<IActionResult> Delete(int id)
        {
            ApiResposta<CategoriaView> apiRetorno = new ApiResposta<CategoriaView>();

            try
            {
                apiRetorno = await _EstoqueApi.CategoriaBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return _RetornarForm(Operacao.Delete, apiRetorno.dados);
        }

        //POST: Categoria/Form
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(string operacao, [FromForm]CategoriaView categoria)
        {
            Enum.TryParse(operacao, out Operacao operacaoAux);

            if (operacaoAux == Operacao.None || operacaoAux == Operacao.Details)
            {
                ModelState.AddModelError("", "Operação não informada corretamente");
                return _RetornarForm(operacaoAux, categoria);
            }

            if (ModelState.IsValid)
            {
                ApiResposta<CategoriaView> apiRetorno = new ApiResposta<CategoriaView>();

                try
                {
                    if (operacaoAux == Operacao.Create)
                    {
                        apiRetorno = await _EstoqueApi.CategoriaInserir(categoria);
                    }
                    else if (operacaoAux == Operacao.Edit)
                    {
                        apiRetorno = await _EstoqueApi.CategoriaAlterar(categoria.Id, categoria);
                    }
                    else if (operacaoAux == Operacao.Delete)
                    {
                        apiRetorno = await _EstoqueApi.CategoriaExcluir(categoria.Id);
                    }
                }
                catch (Exception ex)
                {
                    RetornarMensagem("Falha durante a comunicação com a API");
                }                

                if (apiRetorno.sucesso)
                {
                    return RedirectToAction("Index");
                }

                RetornarModelStateMensagens(apiRetorno.mensagens);
            }

            return _RetornarForm(operacaoAux, categoria);
        }


        //GET: Categoria/Buscar
        public async Task<IActionResult> Buscar(string pesquisa)
        {
            ApiResposta<IEnumerable<CategoriaView>> categorias = new ApiResposta<IEnumerable<CategoriaView>>();
            try
            {
                categorias = await _EstoqueApi.CategoriaBuscar(pesquisa);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return Json(categorias.dados);
        }

        private IActionResult _RetornarForm(Operacao operacao, CategoriaView categoria)
        {
            //Daria para criar uma view para cada operação, porém com uma view única recebendo a operação,
            //em meu entendimento, a manutenção se torna mais facil

            ViewBag.Operacao = operacao;
            return View("Form", categoria);
        }
    }
}
