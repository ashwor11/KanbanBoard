using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories
{
    public interface IRepository<T> : IQuery<T> where T : Entity
    {
        T? Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include);

        IPaginate<T> GetList(Expression<Func<T, bool>>? predicate,
                  Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
                  Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
                  int size = 10,
                  int index = 0,
                  bool enableTracking = true);

        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);


    }
}
