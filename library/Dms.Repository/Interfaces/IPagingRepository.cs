using Dms.Model.Paging;
using System;
using System.Collections.Generic;

namespace Dms.Repository.Interfaces
{
    public interface IPagingRepository
    {
        IEnumerable<T> Find<T>(DbContext db, PagingTableModel model, out long totalCount);
        IEnumerable<T> Find<T>(DbContext db, PagingJoinModel model, out long totalCount);
        IEnumerable<T> Find<T>(DbContext db, PagingApplyModel model, out long totalCount);
        IEnumerable<T> Find<T>(DbContext db, FullTableModel model);
    }
}
