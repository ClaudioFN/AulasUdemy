using CategoriasMvc.Models;

namespace CategoriasMvc.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaViewModel>> GetCategorias();

    Task<CategoriaViewModel> GetCategoriaPorId(int id);

    Task<CategoriaViewModel> CriarCategoria(CategoriaViewModel categoriaVM);

    Task<bool> AtualizarCategoria(int id, CategoriaViewModel categoriaVM);

    Task<bool> DeletaCategoria(int id);
}
