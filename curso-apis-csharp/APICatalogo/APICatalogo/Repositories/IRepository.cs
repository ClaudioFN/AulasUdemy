using System.Linq.Expressions;

namespace APICatalogo.Repositories;

public interface IRepository<T>
{
    // Principio ISP = clientes nao devem ser forcados a usar metodos que nao utilizam
    Task<IEnumerable<T>> GetAllAsync(); // Volta com todas as entidade do respositorio

    //IQueryable<T> GetAllQ(); // Consulta criada para retornar consulta quando receber o ToList
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate); // Classe Expression = representar expressoes lambida;
                                                 // func = delegate; predicate = criterio de filtro
    T Create(T entity);

    T Update(T entity);

    T Delete(T entity);
}
