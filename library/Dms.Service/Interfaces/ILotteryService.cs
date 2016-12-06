using Dms.Model.Lottery;
using Dms.Model.Paging;
using System.Collections.Generic;

namespace Dms.Service.Interfaces
{
    public interface ILotteryService
    {
        IEnumerable<LotteryCatalogVo> FindCatalogDataList(PagingTableModel model, out long totalCount);
    }
}
