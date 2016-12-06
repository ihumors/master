using Dms.Model.Lottery;
using Dms.Model.Paging;
using Dms.Repository;
using Dms.Repository.Interfaces;
using Dms.Service.Interfaces;
using System.Collections.Generic;

namespace Dms.Service.Implements
{
    public class LotteryService: ILotteryService, IDependency
    {
        IPagingRepository PagingRepository;
        public LotteryService(IPagingRepository PagingRepository)
        {
            this.PagingRepository = PagingRepository;
        }
        public IEnumerable<LotteryCatalogVo> FindCatalogDataList(PagingTableModel model, out long totalCount)
        {
            return PagingRepository.Find<LotteryCatalogVo>(DbStorages.LotteryWriterConnection, model, out totalCount);
        }
    }
}
