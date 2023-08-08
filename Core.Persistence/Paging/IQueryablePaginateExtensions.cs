using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging
{
    public static class QueryablePaginateExtensions
    {
        public static IPaginate<T> ToPaginate<T>(this IQueryable<T> queryable, int index, int size, int from = 0, CancellationToken cancellationToken = default)
        {
            if (from > index) throw new ArgumentException($"From: {from} must be smaller or equal to Index:{index}");

            int count = queryable.Count();
            List<T> items = queryable.Skip((index - from) * size).Take(size).ToList();
            int pages = (int)Math.Ceiling((double)count / (double)size);


            return new Paginate<T>(from, index, size, items, count, pages);

        }

        public static async Task<IPaginate<T>> ToPaginateAsync<T>(this IQueryable<T> queryable, int index, int size, int from = 0, CancellationToken cancellationToken = default)
        {
            if (from > index) throw new ArgumentException($"From: {from} must be smaller or equal to Index:{index}");
            int count = queryable.Count();
            List<T> items = await queryable.Skip((index - from) * size).Take(size).ToListAsync();
            int pages = (int)Math.Ceiling((double)count / (double)size);

            return new Paginate<T>(from, index, size, items, count, pages);
        }
    }
}
