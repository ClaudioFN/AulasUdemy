using APICatalogo.Models;

namespace APICatalogo.Repositories;

public interface IUnitOfWork
{
    IProdutoRepository ProdutoRepository { get; }
    // ou 
    //IRepository<Produto> ProdutoRepository { get; }

    ICategoriaInterface CategoriaRepository { get; }
    // ou 
    //IRepository<Categoria> CategoriaRepository { get; } - menos maleavel

    void Commit();
}
