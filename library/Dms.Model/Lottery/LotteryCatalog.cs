using System;

namespace Dms.Model.Lottery
{
    public class LotteryCatalog
    {
        public int lottery_catalog_id { get; set; }
        public string name { get; set; }
        public bool is_enabled { get; set; }
        public bool is_deleted { get; set; }
        public DateTime create_time { get; set; }
        public string created_by { get; set; }
        public DateTime modify_time { get; set; }
        public string modified_by { get; set; }
        public byte[] row_version { get; set; }
    }
    public class LotteryCatalogDto: LotteryCatalog
    {
        public byte action_method { get; set; }
    }
    public class LotteryCatalogVo: LotteryCatalog
    {
        public long row_number { get; set; }
    }
}
