using Estoque.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estoque.Negocio.Interfaces
{
    public interface IProdutoRepositorio: IRepositorio<Produto>
    {
        Produto BuscarPorId(int id, bool referenciado = false);
        IEnumerable<Produto> BuscarTodos(bool trazerCategoria = true);
    }
}
