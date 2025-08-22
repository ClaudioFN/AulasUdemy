using APICatalogo.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public class Repository<T>: IRepository<T> where T : class // Forcando a T ser uma classe
{
    protected readonly AppDbContext _context; // Protected = acessivel pelas classes derivadas e aqui

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAllE()
    {
        return _context.Set<T>().AsNoTracking().ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        //_context.SaveChanges();

        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        //_context.Entry(entity).State = EntityState.Modified;
        //_context.SaveChanges();
        return entity;  
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        //_context.SaveChanges();
        return entity;
    }

}
