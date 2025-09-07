using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories;

public interface ICategoriaInterface : IRepository<Categoria>
{
    /*IEnumerable<Categoria> GetCategorias();

    Categoria GetCategoria(int id);

    Categoria Create(Categoria categoria);

    Categoria Update(Categoria categoria);

    Categoria Delete(int id);*/

    Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParams);

    Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams);
}
