using Estoque.Negocio.Entidades;
using Estoque.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Estoque.Negocio.Repositorios
{
    public interface ICategoriaRepositorio: IRepositorio<Categoria>
    {
        Categoria BuscarPorId(int id, bool referenciado = false);
        IEnumerable<Categoria> Buscar(string pesquisa = default);
    }
}
