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
    public class ProdutoController : PrincipalController
    {
        private readonly IEstoqueApi _EstoqueApi;

        public ProdutoController(IEstoqueApi estoqueApi)
        {
            _EstoqueApi = estoqueApi;
        }

        //GET: Produto
        public async Task<IActionResult> Index()
        {
            ApiResposta<IEnumerable<ProdutoView>> produtos = new ApiResposta<IEnumerable<ProdutoView>>();

            try
            {
                produtos = await _EstoqueApi.ProdutoBuscarTodos();
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }
            
            return View(produtos.dados);
        }

        //GET: Produto/Create
        public IActionResult Create()
        {
            var produto = new ProdutoView();
            produto.Status = TipoStatus.Ativo;
            produto.Tipo = TipoProduto.Revenda;
            return _RetornarForm(Operacao.Create, produto);
        }

        //GET: Produto/Details
        public async Task<IActionResult> Details(int id)
        {
            ApiResposta<ProdutoView> apiRetorno = new ApiResposta<ProdutoView>();

            try
            {
                apiRetorno = await _EstoqueApi.ProdutoBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }            

            return _RetornarForm(Operacao.Details, apiRetorno.dados);
        }

        //GET: Produto/Edit
        public async Task<IActionResult> Edit(int id)
        {
            ApiResposta<ProdutoView> apiRetorno = new ApiResposta<ProdutoView>();

            try
            {
                apiRetorno = await _EstoqueApi.ProdutoBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return _RetornarForm(Operacao.Edit, apiRetorno.dados);
        }

        //GET: Produto/Delete
        public async Task<IActionResult> Delete(int id)
        {
            ApiResposta<ProdutoView> apiRetorno = new ApiResposta<ProdutoView>();

            try
            {
                apiRetorno = await _EstoqueApi.ProdutoBuscarPorId(id);
                RetornarModelStateMensagens(apiRetorno.mensagens);
            }
            catch (Exception ex)
            {
                RetornarMensagem("Falha durante a comunicação com a API");
            }

            return _RetornarForm(Operacao.Delete, apiRetorno.dados);
        }

        //POST: Produto/Form
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Form(string operacao, [FromForm] ProdutoView produto)
        {
            Enum.TryParse(operacao, out Operacao operacaoAux);

            if (operacaoAux == Operacao.None || operacaoAux == Operacao.Details)
            {
                ModelState.AddModelError("", "Operação não informada corretamente");
                return _RetornarForm(operacaoAux, produto);
            }

            if (ModelState.IsValid)
            {
                ApiResposta<ProdutoView> apiRetorno = new ApiResposta<ProdutoView>();

                try
                {
                    if (operacaoAux == Operacao.Create)
                    {
                        apiRetorno = await _EstoqueApi.ProdutoInserir(produto);
                    }
                    else if (operacaoAux == Operacao.Edit)
                    {
                        apiRetorno = await _EstoqueApi.ProdutoAlterar(produto.Id, produto);
                    }
                    else if (operacaoAux == Operacao.Delete)
                    {
                        apiRetorno = await _EstoqueApi.ProdutoExcluir(produto.Id);
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

            return _RetornarForm(operacaoAux, produto);
        }

        private IActionResult _RetornarForm(Operacao operacao, ProdutoView produto)
        {
            //Daria para criar uma view para cada operação, porém com uma view única recebendo a operação,
            //em meu entendimento, a manutenção se torna mais facil

            ViewBag.Operacao = operacao;
            return View("Form", produto);
        }
    }
}
