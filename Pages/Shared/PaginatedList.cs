using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient.Pages.Shared
{    
    public class PaginatedList<T> : List<T> where T : BaseDto
    { 
        public PaginatedList(List<T>items, int count, int pageIndex, int pageSize)
        {
            AddRange(items);
            var rest = count % pageSize;
            LastPage = (rest != 0) ? count / pageSize : (count / pageSize) - 1;
            TypicalEntry = items.FirstOrDefault();
            PageIndex = pageIndex;
        }
        public T TypicalEntry { get; set; }
        public int PageIndex { get; set; }
        public int LastPage { get; set; }
        public bool HasPrevious()
        {
            return (PageIndex > 0);
        }
        public bool HasNext()
        {
            return (PageIndex < LastPage);
        }   
    }
}
