using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.DTOs
{
    public class PaginatedResult<TEntity>
    {
        public PaginatedResult(int count, int pageSize, int pageIndex, IEnumerable<TEntity> data)
        {
            Count = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
        }

        public int Count { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<TEntity> Data { get; set; }

    }
}
