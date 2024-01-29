using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsService.Helpers
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, List<T> data)
        {
            PageIndex = pageIndex;
            Count = count;
            PageSize = pageSize;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public List<T> Data { get; set; }


    }
}
