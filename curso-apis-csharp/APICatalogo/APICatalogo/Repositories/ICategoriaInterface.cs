using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface ICategoriaInterface : IRepository<Categoria>
{
    /*IEnumerable<Categoria> GetCategorias();

    Categoria GetCategoria(int id);

    Categoria Create(Categoria categoria);

    Categoria Update(Categoria categoria);

    Categoria Delete(int id);*/

    PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParams);
}
