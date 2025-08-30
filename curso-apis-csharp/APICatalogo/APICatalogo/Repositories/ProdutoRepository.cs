using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    // private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context) : base(context)
    {

    }

    /*public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        return GetAllE().OrderBy(p => p.Nome)
            .Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize)
            .Take(produtosParams.PageSize).ToList();
    }
    */

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAllE().OrderBy(p => p.ProdutoId).AsQueryable();

        var produtosOrdenados = PagedList<Produto>.ToPageList(produtos, produtosParams.PageNumber, produtosParams.PageSize);

        return produtosOrdenados;
    }
    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAllE().Where(c => c.ProdutoId == id);
    }

    /*public IQueryable<Produto> GetProdutos()
    {
        return _context.Produtos;
    }

    public Produto GetProduto(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto == null)
        {
            throw new InvalidOperationException("Produto não encontrado!");
        }

        return produto;

    }

    public Produto Create(Produto produto)
    {
        if (produto == null)
        {
            throw new InvalidOperationException("Produto não encontrado!");
        }

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return produto;
    }

    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);

        if(produto is not null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges(); 

            return true;
        }

        return false;
    }


    public bool Update(Produto produto)
    {
        if (produto == null)
        {
            throw new InvalidOperationException("Produto não encontrado!");
        }

        if (_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId))
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();

            return true;
        }

        return false;
    }*/
}
