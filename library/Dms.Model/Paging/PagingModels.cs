using Dms.Model.Enums;

namespace Dms.Model.Paging
{
    public class FullTableModel
    {
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string OrderByColumns { get; set; }
        public string Where { get; set; }
    }
    public class PagingTableModel : FullTableModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
    public class PagingJoinModel : PagingTableModel
    {
        private PagingJoinTypeEnum joinMode = PagingJoinTypeEnum.Inner;
        public PagingJoinTypeEnum JoinMode
        {
            get { return joinMode; }
            set { joinMode = value; }
        }
        public string JoinTableName { get; set; }
        public string JoinColumnsName { get; set; }
        public string JoinWhere { get; set; }
    }
    public class PagingApplyModel : PagingTableModel
    {
        private PagingApplyTypeEnum applyMode = PagingApplyTypeEnum.Cross;
        public PagingApplyTypeEnum ApplyMode
        {
            get { return applyMode; }
            set { applyMode = value; }
        }
        public string ApplyColumnsName { get; set; }
        public string ApplyFunctionCmd { get; set; }
    }
    
}
