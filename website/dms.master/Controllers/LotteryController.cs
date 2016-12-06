using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dms.Model.Lottery;
using Dms.Master.Models;
using Dms.Service.Interfaces;
using Dms.Repository.Interfaces;

namespace Dms.Master.Controllers
{
    [AllowAnonymous]
    public class LotteryController : BaseController
    {
        ILotteryService LotteryService;
        IPagingRepository PagingRepository;
        public LotteryController(ILotteryService LotteryService, IPagingRepository PagingRepository)
        {
            this.LotteryService = LotteryService;
            this.PagingRepository = PagingRepository;
        }
        // GET: Lottery
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Catalog()
        {
            return View();
        }
        public ActionResult _PartialCatalogDataList()
        {
            return View();
        }
        public ActionResult _PartialCatalogEditForm(int lotteryCatalogId)
        {
            LotteryCatalogVo model = new LotteryCatalogVo();
            return View(model);
        }
        public ActionResult _PartialEditLotteryStrategy(int lotteryCatalogId)
        {
            LotteryStrategyVo model = new LotteryStrategyVo();
            return View(model);
        }
        public ActionResult BlackUser()
        {
            return View();
        }
        public ActionResult Information()
        {
            return View();
        }
        public ActionResult PrizeType()
        {
            return View();
        }
        public ActionResult Report()
        {
            return View();
        }
        public JsonResult LoadLotteryCatalogDataList(PaginationModel paginationModel)
        {
            long totalCount = 0;

            IEnumerable<LotteryCatalogVo> list = LotteryService.FindCatalogDataList(new Model.Paging.PagingTableModel()
            {
                PageIndex = paginationModel.pageIndex,
                PageSize = paginationModel.pageSize,
                ColumnsName = "*",
                OrderByColumns = string.Format("{0} {1}",paginationModel.sort, paginationModel.sortBy),
                TableName = "[dbo].[lottery_catalog]",
                Where = string.IsNullOrEmpty(paginationModel.key) ? null : string.Format("[name]='{0}'", paginationModel.key.Replace("'", "''"))
            }, out totalCount);

            paginationModel.totalCount = totalCount;

            JsonResult jsonResult = Json(new {
                //breadCrumb="",
                datalist = RenderViewToString("_PartialCatalogDataList", list, this.ControllerContext),
                pagination= RenderViewToString("_PartialPagination", paginationModel, this.ControllerContext)
            },JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
    }
}