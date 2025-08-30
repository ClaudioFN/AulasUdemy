using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    /*IQueryable<Produto> GetProdutos();

    Produto GetProduto(int id);

    Produto Create(Produto produto);

    bool Update(Produto produto);

    bool Delete(int id);*/

    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    // Aula 112 trocou o de cima para o de baixo
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
