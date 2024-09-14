using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Interfaces
{
    public interface IPaginatedList<TEntity>
    {
        IReadOnlyCollection<TEntity> Items { get; }
        int TotalItems { get; }
        int CurrentPage { get; }
        int TotalPages { get; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
