using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaInterface
{
    // private readonly AppDbContext _context;

    public CategoriaRepository(AppDbContext context): base(context)
    {

    }

    public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriasParams)
    {
        var categorias = await GetAllAsync();

        var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();

        //var categoriasOrdenados = IPagedList<Categoria>.ToPageListAsync(categoriasOrdenadas, categoriasParams.PageNumber, categoriasParams.PageSize);
        var categoriasOrdenados = await categoriasOrdenadas.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasOrdenados;
    }

    public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
    {
        var categorias = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        //var categoriasFiltradas = PagedList<Categoria>.ToPageList(categorias.AsQueryable(), categoriasParams.PageNumber, categoriasParams.PageSize);
        var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasFiltradas;
    }

    /*public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias.ToList();
    }

    public Categoria GetCategoria(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    }

    public Categoria Create(Categoria categoria)
    {
        if(categoria is null)
        {
            throw new ArgumentNullException(nameof(categoria));
        }

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }

    public Categoria Update(Categoria categoria)
    {
        if (categoria is null)
        {
            throw new ArgumentNullException(nameof(categoria));
        }

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return categoria;   
    }

    public Categoria Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);
        if (categoria is null)
        {
            throw new ArgumentNullException(nameof(categoria));
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return categoria;

    }
    */



}
