using Estoque.App.Models;
using Estoque.Negocio.Utilidades;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estoque.App.Utilities
{
    //Dava para fazer com httpclient, porém o refit deixa bem mais facil a implementação
    public interface IEstoqueApi
    {
        [Get("/categoria")]
        Task<ApiResposta<IEnumerable<CategoriaView>>> CategoriaBuscar(string pesquisa = default);

        [Get("/categoria/{id}")]
        Task<ApiResposta<CategoriaView>> CategoriaBuscarPorId(int id);

        [Post("/categoria")]
        Task<ApiResposta<CategoriaView>> CategoriaInserir(CategoriaView categoriaView);

        [Put("/categoria/{id}")]
        Task<ApiResposta<CategoriaView>> CategoriaAlterar(int id, [Body]CategoriaView categoriaView);

        [Delete("/categoria/{id}")]
        Task<ApiResposta<CategoriaView>> CategoriaExcluir(int id);


        [Get("/produto")]
        Task<ApiResposta<IEnumerable<ProdutoView>>> ProdutoBuscarTodos();

        [Get("/produto/{id}")]
        Task<ApiResposta<ProdutoView>> ProdutoBuscarPorId(int id);

        [Post("/produto")]
        Task<ApiResposta<ProdutoView>> ProdutoInserir(ProdutoView categoriaView);

        [Put("/produto/{id}")]
        Task<ApiResposta<ProdutoView>> ProdutoAlterar(int id, [Body] ProdutoView categoriaView);

        [Delete("/produto/{id}")]
        Task<ApiResposta<ProdutoView>> ProdutoExcluir(int id);
    }
}
