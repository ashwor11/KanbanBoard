using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging
{
    public class Paginate<T> : IPaginate<T>
    {


        public Paginate(int from, int index, int size, List<T> items, int count, int pages)
        {
            if (from > index) throw new ArgumentException($"From: {from} must be smaller or equal to Index:{index}");

            From = from;
            Index = index;
            Size = size;
            Items = items;
            Count = count;
            Pages = pages;

            HasNext = Index == Pages ? false : true;
            HasPrevious = Index == From ? false : true;

        }

        public int From { get; }
        public int Index { get; }
        public int Size { get; }
        public IList<T> Items { get; }
        public int Count { get; }
        public int Pages { get; }
        public bool HasNext { get; }
        public bool HasPrevious { get; }

    }
}
