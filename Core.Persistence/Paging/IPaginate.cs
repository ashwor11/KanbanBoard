using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging
{
    public interface IPaginate<T>
    {

        public int From { get; }
        public int Index { get; }
        public int Size { get; }
        public int Pages { get; }
        public int Count { get; }
        IList<T> Items { get; }
        public bool HasNext { get; }
        public bool HasPrevious { get; }


    }
}
